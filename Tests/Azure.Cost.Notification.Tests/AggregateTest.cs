namespace Azure.Cost.Notification.Tests;

using System;
using System.Threading.Tasks;
using Application.Domain.Models;
using ChainingAssertion;
using Domain.ValueObjects;
using Infrastructure.RestApi;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class AggregateTest
{
    private static readonly string GetAccessTokenActivityName = $"{nameof(SharedActivity)}_{nameof(SharedActivity.GetAccessToken)}";

    private static readonly string SendChatworkActivityName = $"{nameof(SharedActivity)}_{nameof(SharedActivity.SendChatwork)}";

    private static readonly string DailyTotalCostActivityName = $"{nameof(SharedActivity)}_{nameof(SharedActivity.DailyTotalCost)}";

    private static readonly string WeeklyTotalCostActivityName = $"{nameof(SharedActivity)}_{nameof(SharedActivity.WeeklyTotalCost)}";

    private static readonly string MonthlyTotalCostActivityName = $"{nameof(SharedActivity)}_{nameof(SharedActivity.MonthlyTotalCost)}";

    private static readonly string FormatChatworkMessageActivityName = $"{nameof(SharedActivity)}_{nameof(SharedActivity.FormatChatworkMessage)}";

    private readonly Aggregate _target;

    private readonly Mock<ILogger> _logger = new();

    private readonly TestFactory _testFactory = new();

    public AggregateTest()
    {
        _target = new Aggregate();
    }

    private sealed class TestFactory
    {
        public TestFactory()
        {
            Context = new Mock<IDurableOrchestrationContext>();
        }

        public Mock<IDurableOrchestrationContext> Context { get; }
    }

    [Fact]
    public async Task Test_Orchestrator_集計した情報を送信した結果を取得できること()
    {
        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<ChatworkMessage>(FormatChatworkMessageActivityName, It.IsAny<TotalCostResult[]>()))
                    .ReturnsAsync(() => new ChatworkMessage($"{nameof(Test_Orchestrator_集計した情報を送信した結果を取得できること)}"));

        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<ChatworkSendResult>(SendChatworkActivityName, It.IsAny<ChatworkMessage>()))
                    .ReturnsAsync((string functionName, ChatworkMessage x) =>
                     {
                         // 例外がスローされた場合、受け取ったメッセージは例外インスタンスのメッセージとなる。
                         // なので、フォーマットしたときの戻り値のメッセージを取得できていればこのテストは正常と判断できる。
                         x.ToString().Is($"{nameof(Test_Orchestrator_集計した情報を送信した結果を取得できること)}");
                         return new ChatworkSendResult(x, "12345");
                     });

        var result = await _target.Orchestrator(_testFactory.Context.Object, _logger.Object);
        
        // 最後まで実行できているかどうかを確認する。
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<ChatworkMessage>(FormatChatworkMessageActivityName, It.IsAny<TotalCostResult[]>()), Times.Once);
    }

    [Fact]
    public async Task Test_Orchestrator_アクセストークン取得に失敗した場合に送信した結果を取得できること()
    {
        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<AzureAuthentication>(GetAccessTokenActivityName, It.IsAny<AzureAccessTokenRequest>()))
                    .Throws<ApplicationException>();

        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<ChatworkSendResult>(SendChatworkActivityName, It.IsAny<ChatworkMessage>()))
                    .ReturnsAsync((string functionName, ChatworkMessage x) => new ChatworkSendResult(x, "123"));

        var result = await _target.Orchestrator(_testFactory.Context.Object, _logger.Object);

        Assert.Contains("Azure 利用料金の通知に失敗しました。", result);

        // 例外だけだとどのタイミングか判断できないので、明示的に失敗した機能まで実行されていることは確認する。
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<AzureAuthentication>(GetAccessTokenActivityName, It.IsAny<AzureAccessTokenRequest>()), Times.Once);
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<TotalCostResult>(DailyTotalCostActivityName, It.IsAny<AzureAuthentication>()), Times.Never);
    }

    [Fact]
    public async Task Test_Orchestrator_利用料金取得に失敗した場合に送信した結果を取得できること()
    {
        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<TotalCostResult>(WeeklyTotalCostActivityName, It.IsAny<AzureAuthentication>()))
                    .Throws<AzureRestApiException>();

        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<ChatworkSendResult>(SendChatworkActivityName, It.IsAny<ChatworkMessage>()))
                    .ReturnsAsync((string functionName, ChatworkMessage x) => new ChatworkSendResult(x, "123"));

        var result = await _target.Orchestrator(_testFactory.Context.Object, _logger.Object);
        
        Assert.Contains("Azure 利用料金の通知に失敗しました。", result);

        // 例外だけだとどのタイミングか判断できないので、明示的に失敗した機能まで実行されていることは確認する。
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<TotalCostResult>(WeeklyTotalCostActivityName, It.IsAny<AzureAuthentication>()), Times.Once);
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<TotalCostResult>(MonthlyTotalCostActivityName, It.IsAny<AzureAuthentication>()), Times.Never);
    }

    [Fact]
    public async Task Test_Orchestrator_メッセージフォーマットに失敗した場合に送信した結果を取得できること()
    {
        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<ChatworkMessage>(FormatChatworkMessageActivityName, It.IsAny<TotalCostResult[]>()))
                    .Throws<ApplicationException>();

        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<ChatworkSendResult>(SendChatworkActivityName, It.IsAny<ChatworkMessage>()))
                    .ReturnsAsync((string functionName, ChatworkMessage x) => new ChatworkSendResult(x, "123"));

        var result = await _target.Orchestrator(_testFactory.Context.Object, _logger.Object);

        Assert.Contains("Azure 利用料金の通知に失敗しました。", result);

        // 例外だけだとどのタイミングか判断できないので、明示的に失敗した機能まで実行されていることは確認する。
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<ChatworkMessage>(FormatChatworkMessageActivityName, It.IsAny<TotalCostResult[]>()), Times.Once);
    }

    [Fact]
    public async Task Test_Orchestrator_メッセージ送信に失敗した場合に例外がスローされること()
    {
        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<ChatworkMessage>(FormatChatworkMessageActivityName, It.IsAny<TotalCostResult[]>()))
                    .Throws<ArgumentException>();

        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<ChatworkSendResult>(SendChatworkActivityName, It.IsAny<ChatworkMessage>()))
                    .Throws<ApplicationException>();

        // Chatwork のメッセージ送信で失敗した場合はどうしようもないのでそのまま例外投げるだけ。
        (await Record.ExceptionAsync(() => _target.Orchestrator(_testFactory.Context.Object, _logger.Object)))
                    .IsInstanceOf<ApplicationException>();
    }
}