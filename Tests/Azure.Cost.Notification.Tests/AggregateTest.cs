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
    public async Task Test_Orchestrator_�W�v�������𑗐M�������ʂ��擾�ł��邱��()
    {
        var durableOrchestrationContextMock = new Mock<IDurableOrchestrationContext>();
        durableOrchestrationContextMock
               .Setup(x => x.CallActivityAsync<AzureAuthentication>($"{nameof(SharedActivity)}_{nameof(SharedActivity.GetAccessToken)}", It.IsAny<AzureAccessTokenRequest>()))
               .Throws<ApplicationException>();
        durableOrchestrationContextMock
               .Setup(x => x.CallActivityAsync<ChatworkSendResult>($"{nameof(SharedActivity)}_{nameof(SharedActivity.SendChatwork)}", It.IsAny<ChatworkMessage>()))
               .ReturnsAsync((string functionName, ChatworkMessage x) => new ChatworkSendResult(x, "123"));

        var result = await _target.Orchestrator(durableOrchestrationContextMock.Object, _logger.Object);

        Assert.Contains("Azure ���p�����̒ʒm�Ɏ��s���܂����B", result);
    }

    public void Test_Orchestrator_�A�N�Z�X�g�[�N���擾�Ɏ��s�����ꍇ�ɑ��M�������ʂ��擾�ł��邱��()
    {

    }

    public void Test_Orchestrator_���p�����擾�Ɏ��s�����ꍇ�ɑ��M�������ʂ��擾�ł��邱��()
    {

    }

    public void Test_Orchestrator_���b�Z�[�W�t�H�[�}�b�g�Ɏ��s�����ꍇ�ɑ��M�������ʂ��擾�ł��邱��()
    {

    }

    public void Test_Orchestrator_���b�Z�[�W���M�Ɏ��s�����ꍇ�ɗ�O���X���[����邱��()
    {

    }
}