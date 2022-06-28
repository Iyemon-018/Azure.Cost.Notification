namespace Azure.Cost.Notification.Tests.Application.Domain.Services;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChainingAssertion;
using Moq;
using Notification.Application.Domain.Services;
using Notification.Domain.Entities;
using Notification.Domain.Models;
using Notification.Domain.Repositories;
using Notification.Infrastructure.ChatworkApi;
using Xunit;

public class SendMessageServiceTest
{
    private readonly SendMessageService _target;

    private readonly TestFactory _testFactory;

    internal sealed class TestFactory
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly Mock<IMessageSendRepository> _messageSendRepositoryMock;

        public TestFactory()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _messageSendRepositoryMock = new Mock<IMessageSendRepository>();

            _unitOfWorkMock.Setup(x => x.MessageSendRepository)
                           .Returns(() => _messageSendRepositoryMock.Object);

            SetupSendResult((m, _) => new ChatworkSendResult(m, "test"));
        }

        public  IUnitOfWork UnitOfWork => _unitOfWorkMock.Object;

        public void SetupSendResult(Func<ChatworkMessage, CancellationToken, ChatworkSendResult> mockFunc)
        {
            _messageSendRepositoryMock.Setup(x => x.SendAsync(It.IsAny<ChatworkMessage>(), It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(mockFunc);
        }
    }

    public SendMessageServiceTest()
    {
        _testFactory = new TestFactory();
        _target      = new SendMessageService(_testFactory.UnitOfWork);
    }

    [Fact]
    public async Task Test_ExecuteAsync_１つのメッセージ送信に成功して結果が期待した値であること()
    {
        var messageId = $"{DateTime.Now:fffssmm}";
        var message   = new[] {new ChatworkMessage(982021, nameof(Test_ExecuteAsync_１つのメッセージ送信に成功して結果が期待した値であること))};
        var expected = new Queue<string>(new[]
                                         {
                                             $"Send:{messageId}, Room:982021, {nameof(Test_ExecuteAsync_１つのメッセージ送信に成功して結果が期待した値であること)}"
                                         });
        _testFactory.SetupSendResult((m, _) => new ChatworkSendResult(m, messageId));

        await foreach (var result in _target.ExecuteAsync("test-Token", message))
        {
            result.Log().Is(expected.Dequeue());
        }
    }

    [Fact]
    public async Task Test_ExecuteAsync_３つのメッセージ送信に成功して結果が期待した値であること()
    {
        var messageId = $"{DateTime.Now:fffssmm}";
        var message = new[]
                      {
                          new ChatworkMessage(122209, nameof(Test_ExecuteAsync_３つのメッセージ送信に成功して結果が期待した値であること) + "1")
                        , new ChatworkMessage(128219, nameof(Test_ExecuteAsync_３つのメッセージ送信に成功して結果が期待した値であること) + "2")
                        , new ChatworkMessage(122209, nameof(Test_ExecuteAsync_３つのメッセージ送信に成功して結果が期待した値であること) + "3")
                      };
        var expected = new Queue<string>(new[]
                                         {
                                             $"Send:{messageId}, Room:122209, {nameof(Test_ExecuteAsync_３つのメッセージ送信に成功して結果が期待した値であること)}" + "1"
                                           , $"Send:{messageId}, Room:128219, {nameof(Test_ExecuteAsync_３つのメッセージ送信に成功して結果が期待した値であること)}" + "2"
                                           , $"Send:{messageId}, Room:122209, {nameof(Test_ExecuteAsync_３つのメッセージ送信に成功して結果が期待した値であること)}" + "3"
                                         });
        _testFactory.SetupSendResult((m, _) => new ChatworkSendResult(m, messageId));

        await foreach (var result in _target.ExecuteAsync("Test-Token", message))
        {
            result.Log().Is(expected.Dequeue());
        }
    }
}