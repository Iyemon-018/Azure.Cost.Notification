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
    public async Task Test_Orchestrator_�W�v�������𑗐M�������ʂ��擾�ł��邱��()
    {
        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<ChatworkMessage>(FormatChatworkMessageActivityName, It.IsAny<TotalCostResult[]>()))
                    .ReturnsAsync(() => new ChatworkMessage($"{nameof(Test_Orchestrator_�W�v�������𑗐M�������ʂ��擾�ł��邱��)}"));

        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<ChatworkSendResult>(SendChatworkActivityName, It.IsAny<ChatworkMessage>()))
                    .ReturnsAsync((string functionName, ChatworkMessage x) =>
                     {
                         // ��O���X���[���ꂽ�ꍇ�A�󂯎�������b�Z�[�W�͗�O�C���X�^���X�̃��b�Z�[�W�ƂȂ�B
                         // �Ȃ̂ŁA�t�H�[�}�b�g�����Ƃ��̖߂�l�̃��b�Z�[�W���擾�ł��Ă���΂��̃e�X�g�͐���Ɣ��f�ł���B
                         x.ToString().Is($"{nameof(Test_Orchestrator_�W�v�������𑗐M�������ʂ��擾�ł��邱��)}");
                         return new ChatworkSendResult(x, "12345");
                     });

        var result = await _target.Orchestrator(_testFactory.Context.Object, _logger.Object);
        
        // �Ō�܂Ŏ��s�ł��Ă��邩�ǂ������m�F����B
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<ChatworkMessage>(FormatChatworkMessageActivityName, It.IsAny<TotalCostResult[]>()), Times.Once);
    }

    [Fact]
    public async Task Test_Orchestrator_�A�N�Z�X�g�[�N���擾�Ɏ��s�����ꍇ�ɑ��M�������ʂ��擾�ł��邱��()
    {
        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<AzureAuthentication>(GetAccessTokenActivityName, It.IsAny<AzureAccessTokenRequest>()))
                    .Throws<ApplicationException>();

        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<ChatworkSendResult>(SendChatworkActivityName, It.IsAny<ChatworkMessage>()))
                    .ReturnsAsync((string functionName, ChatworkMessage x) => new ChatworkSendResult(x, "123"));

        var result = await _target.Orchestrator(_testFactory.Context.Object, _logger.Object);

        Assert.Contains("Azure ���p�����̒ʒm�Ɏ��s���܂����B", result);

        // ��O�������Ƃǂ̃^�C�~���O�����f�ł��Ȃ��̂ŁA�����I�Ɏ��s�����@�\�܂Ŏ��s����Ă��邱�Ƃ͊m�F����B
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<AzureAuthentication>(GetAccessTokenActivityName, It.IsAny<AzureAccessTokenRequest>()), Times.Once);
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<TotalCostResult>(DailyTotalCostActivityName, It.IsAny<AzureAuthentication>()), Times.Never);
    }

    [Fact]
    public async Task Test_Orchestrator_���p�����擾�Ɏ��s�����ꍇ�ɑ��M�������ʂ��擾�ł��邱��()
    {
        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<TotalCostResult>(WeeklyTotalCostActivityName, It.IsAny<AzureAuthentication>()))
                    .Throws<AzureRestApiException>();

        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<ChatworkSendResult>(SendChatworkActivityName, It.IsAny<ChatworkMessage>()))
                    .ReturnsAsync((string functionName, ChatworkMessage x) => new ChatworkSendResult(x, "123"));

        var result = await _target.Orchestrator(_testFactory.Context.Object, _logger.Object);
        
        Assert.Contains("Azure ���p�����̒ʒm�Ɏ��s���܂����B", result);

        // ��O�������Ƃǂ̃^�C�~���O�����f�ł��Ȃ��̂ŁA�����I�Ɏ��s�����@�\�܂Ŏ��s����Ă��邱�Ƃ͊m�F����B
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<TotalCostResult>(WeeklyTotalCostActivityName, It.IsAny<AzureAuthentication>()), Times.Once);
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<TotalCostResult>(MonthlyTotalCostActivityName, It.IsAny<AzureAuthentication>()), Times.Never);
    }

    [Fact]
    public async Task Test_Orchestrator_���b�Z�[�W�t�H�[�}�b�g�Ɏ��s�����ꍇ�ɑ��M�������ʂ��擾�ł��邱��()
    {
        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<ChatworkMessage>(FormatChatworkMessageActivityName, It.IsAny<TotalCostResult[]>()))
                    .Throws<ApplicationException>();

        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<ChatworkSendResult>(SendChatworkActivityName, It.IsAny<ChatworkMessage>()))
                    .ReturnsAsync((string functionName, ChatworkMessage x) => new ChatworkSendResult(x, "123"));

        var result = await _target.Orchestrator(_testFactory.Context.Object, _logger.Object);

        Assert.Contains("Azure ���p�����̒ʒm�Ɏ��s���܂����B", result);

        // ��O�������Ƃǂ̃^�C�~���O�����f�ł��Ȃ��̂ŁA�����I�Ɏ��s�����@�\�܂Ŏ��s����Ă��邱�Ƃ͊m�F����B
        _testFactory.Context
                    .Verify(x => x.CallActivityAsync<ChatworkMessage>(FormatChatworkMessageActivityName, It.IsAny<TotalCostResult[]>()), Times.Once);
    }

    [Fact]
    public async Task Test_Orchestrator_���b�Z�[�W���M�Ɏ��s�����ꍇ�ɗ�O���X���[����邱��()
    {
        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<ChatworkMessage>(FormatChatworkMessageActivityName, It.IsAny<TotalCostResult[]>()))
                    .Throws<ArgumentException>();

        _testFactory.Context
                    .Setup(x => x.CallActivityAsync<ChatworkSendResult>(SendChatworkActivityName, It.IsAny<ChatworkMessage>()))
                    .Throws<ApplicationException>();

        // Chatwork �̃��b�Z�[�W���M�Ŏ��s�����ꍇ�͂ǂ����悤���Ȃ��̂ł��̂܂ܗ�O�����邾���B
        (await Record.ExceptionAsync(() => _target.Orchestrator(_testFactory.Context.Object, _logger.Object)))
                    .IsInstanceOf<ApplicationException>();
    }
}