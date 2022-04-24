namespace Azure.Cost.Notification.Tests;

using System;
using System.Threading.Tasks;
using Domain.ValueObjects;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Models;
using Moq;
using Xunit;

public class AggregateTest
{
    private readonly Aggregate _target;

    private readonly Mock<ILogger> _logger = new();

    public AggregateTest()
    {
        _target = new Aggregate();
    }

    [Fact]
    public async Task Test_Orchestrator_集計した情報を送信した結果を取得できること()
    {
        var durableOrchestrationContextMock = new Mock<IDurableOrchestrationContext>();
        durableOrchestrationContextMock
               .Setup(x => x.CallActivityAsync<AzureAuthentication>($"{nameof(SharedActivity)}_{nameof(SharedActivity.GetAccessToken)}", It.IsAny<AzureAccessTokenRequest>()))
               .Throws<ApplicationException>();
        durableOrchestrationContextMock
               .Setup(x => x.CallActivityAsync<ChatworkSendResult>($"{nameof(SharedActivity)}_{nameof(SharedActivity.SendChatwork)}", It.IsAny<ChatworkMessage>()))
               .ReturnsAsync((string functionName, ChatworkMessage x) => new ChatworkSendResult(x, "123"));

        var result = await _target.Orchestrator(durableOrchestrationContextMock.Object, _logger.Object);

        Assert.Contains("Azure 利用料金の通知に失敗しました。", result);
    }

    public void Test_Orchestrator_アクセストークン取得に失敗した場合に送信した結果を取得できること()
    {

    }

    public void Test_Orchestrator_利用料金取得に失敗した場合に送信した結果を取得できること()
    {

    }

    public void Test_Orchestrator_メッセージフォーマットに失敗した場合に送信した結果を取得できること()
    {

    }

    public void Test_Orchestrator_メッセージ送信に失敗した場合に例外がスローされること()
    {

    }
}