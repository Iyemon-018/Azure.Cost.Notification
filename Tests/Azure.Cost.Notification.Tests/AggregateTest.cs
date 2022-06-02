namespace Azure.Cost.Notification.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ChainingAssertion;
using Domain.ValueObjects;
using Infrastructure.RestApi;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Moq;
using Notification.Application.Domain.Models;
using Xunit;
using Xunit.Abstractions;

public class AggregateTest
{
    private readonly ITestOutputHelper _outputHelper;

    private static readonly string GetAccessTokenActivityName = $"{nameof(SharedActivity)}_{nameof(SharedActivity.GetAccessToken)}";

    private static readonly string SendChatworkActivityName = $"{nameof(SharedActivity)}_{nameof(SharedActivity.SendChatwork)}";

    private static readonly string DailyTotalCostActivityName = $"{nameof(SharedActivity)}_{nameof(SharedActivity.DailyTotalCost)}";

    private static readonly string WeeklyTotalCostActivityName = $"{nameof(SharedActivity)}_{nameof(SharedActivity.WeeklyTotalCost)}";

    private static readonly string MonthlyTotalCostActivityName = $"{nameof(SharedActivity)}_{nameof(SharedActivity.MonthlyTotalCost)}";

    private static readonly string FormatChatworkMessageActivityName = $"{nameof(SharedActivity)}_{nameof(SharedActivity.FormatChatworkMessage)}";

    private readonly Aggregate _target;

    private readonly Mock<ILogger> _logger = new();

    private readonly TestFactory _testFactory = new();

    public AggregateTest(ITestOutputHelper _outputHelper)
    {
        this._outputHelper = _outputHelper;
        _target            = new Aggregate();
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
                    .Setup(x => x.CallActivityAsync<IEnumerable<ChatworkMessage>>(FormatChatworkMessageActivityName, It.IsAny<(int, TotalCostResult[])>()))
                    .ReturnsAsync(() => new[] {new ChatworkMessage(98878, $"{nameof(Test_Orchestrator_集計した情報を送信した結果を取得できること)}")});

        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<IEnumerable<ChatworkSendResult>>(SendChatworkActivityName, It.IsAny<IEnumerable<ChatworkMessage>>()))
                    .ReturnsAsync((string functionName, IEnumerable<ChatworkMessage> messages) =>
                     {
                         // 例外がスローされた場合、受け取ったメッセージは例外インスタンスのメッセージとなる。
                         // なので、フォーマットしたときの戻り値のメッセージを取得できていればこのテストは正常と判断できる。
                         messages.First().ToString().Is($"{nameof(Test_Orchestrator_集計した情報を送信した結果を取得できること)}");
                         return messages.Select(x => new ChatworkSendResult(x, "12345"));
                     });

        var result = await _target.Orchestrator(_testFactory.Context.Object, _logger.Object);

        // 最後まで実行できているかどうかを確認する。
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<IEnumerable<ChatworkMessage>>(FormatChatworkMessageActivityName, It.IsAny<(int, TotalCostResult[])>()), Times.Once);
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<IEnumerable<ChatworkSendResult>>(SendChatworkActivityName, It.IsAny<IEnumerable<ChatworkMessage>>()), Times.Once);
    }

    [Fact]
    public async Task Test_Orchestrator_アクセストークン取得に失敗した場合に送信した結果を取得できること()
    {
        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<AzureAuthentication>(GetAccessTokenActivityName, It.IsAny<AzureAccessTokenRequest>()))
                    .Throws<ApplicationException>();

        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<IEnumerable<ChatworkSendResult>>(SendChatworkActivityName, It.IsAny<IEnumerable<ChatworkMessage>>()))
                    .ReturnsAsync((string functionName, IEnumerable<ChatworkMessage> messages) => messages.Select(x => new ChatworkSendResult(x, "123")));

        var result = await _target.Orchestrator(_testFactory.Context.Object, _logger.Object);

        Assert.Contains("Azure 利用料金の通知に失敗しました。", result);

        // 例外だけだとどのタイミングか判断できないので、明示的に失敗した機能まで実行されていることは確認する。
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<AzureAuthentication>(GetAccessTokenActivityName, It.IsAny<AzureAccessTokenRequest>()), Times.Once);
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<TotalCostResult>(DailyTotalCostActivityName, It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Test_Orchestrator_利用料金取得に失敗した場合に送信した結果を取得できること()
    {
        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<TotalCostResult>(WeeklyTotalCostActivityName, It.IsAny<string>()))
                    .Throws(new AzureRestApiException(HttpStatusCode.Unauthorized, new Uri("https://microsoft.com"), new HttpRequestMessage(), string.Empty));

        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<IEnumerable<ChatworkSendResult>>(SendChatworkActivityName, It.IsAny<IEnumerable<ChatworkMessage>>()))
                    .ReturnsAsync((string functionName, IEnumerable<ChatworkMessage> messages) => messages.Select(x => new ChatworkSendResult(x, "123")));

        var result = await _target.Orchestrator(_testFactory.Context.Object, _logger.Object);

        Assert.Contains("Azure 利用料金の通知に失敗しました。", result);

        // 例外だけだとどのタイミングか判断できないので、明示的に失敗した機能まで実行されていることは確認する。
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<TotalCostResult>(WeeklyTotalCostActivityName, It.IsAny<string>()), Times.Once);
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<TotalCostResult>(MonthlyTotalCostActivityName, It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Test_Orchestrator_メッセージフォーマットに失敗した場合に送信した結果を取得できること()
    {
        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<IEnumerable<ChatworkMessage>>(FormatChatworkMessageActivityName, It.IsAny<(int, TotalCostResult[])>()))
                    .Throws<ApplicationException>();

        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<IEnumerable<ChatworkSendResult>>(SendChatworkActivityName, It.IsAny<IEnumerable<ChatworkMessage>>()))
                    .ReturnsAsync((string functionName, IEnumerable<ChatworkMessage> messages) => messages.Select(x => new ChatworkSendResult(x, "123")));

        var result = await _target.Orchestrator(_testFactory.Context.Object, _logger.Object);

        Assert.Contains("Azure 利用料金の通知に失敗しました。", result);

        // 例外だけだとどのタイミングか判断できないので、明示的に失敗した機能まで実行されていることは確認する。
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<IEnumerable<ChatworkMessage>>(FormatChatworkMessageActivityName, It.IsAny<(int, TotalCostResult[])>()), Times.Once);
    }

    [Fact]
    public async Task Test_Orchestrator_メッセージ送信に失敗した場合に例外がスローされること()
    {
        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<IEnumerable<ChatworkMessage>>(FormatChatworkMessageActivityName, It.IsAny<(int, TotalCostResult[])>()))
                    .Throws<ArgumentException>();

        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<IEnumerable<ChatworkSendResult>>(SendChatworkActivityName, It.IsAny<IEnumerable<ChatworkMessage>>()))
                    .Throws<ApplicationException>();

        // Chatwork のメッセージ送信で失敗した場合はどうしようもないのでそのまま例外投げるだけ。
        (await Record.ExceptionAsync(() => _target.Orchestrator(_testFactory.Context.Object, _logger.Object)))
               .IsInstanceOf<ApplicationException>();
    }

    [Fact]
    public async Task Test_Orchestrator_送信するメッセージ構築のための情報が期待した値であること()
    {
        var messageId = $"{DateTime.Now:ssfffMM}";
        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<IEnumerable<ChatworkMessage>>(FormatChatworkMessageActivityName, It.IsAny<(int, TotalCostResult[])>()))
                    .ReturnsAsync(() => new[] {new ChatworkMessage(118772, nameof(Test_Orchestrator_送信するメッセージ構築のための情報が期待した値であること))});
        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<IEnumerable<ChatworkSendResult>>(SendChatworkActivityName, It.IsAny<IEnumerable<ChatworkMessage>>()))
                    .ReturnsAsync((string functionName, IEnumerable<ChatworkMessage> messages) =>
                     {
                         messages.Count().Is(1);
                         messages.First().ToString().Is(nameof(Test_Orchestrator_送信するメッセージ構築のための情報が期待した値であること));
                         return messages.Select(x => new ChatworkSendResult(x, messageId));
                     });

        var result = await _target.Orchestrator(_testFactory.Context.Object, _logger.Object);

        result.Is(new ChatworkSendResult(new ChatworkMessage(118772, nameof(Test_Orchestrator_送信するメッセージ構築のための情報が期待した値であること)), messageId).Log);
        _outputHelper.WriteLine(result);
    }
}