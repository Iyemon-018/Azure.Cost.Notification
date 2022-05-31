namespace Azure.Cost.Notification.Tests.Application.Domain.Services;

using System;
using System.Linq;
using System.Threading.Tasks;
using ChainingAssertion;
using Notification.Application.Domain.Services;
using Notification.Domain.ValueObjects;
using Xunit;

public class SendMessageServiceTest
{
    private readonly SendMessageService _target;

    public SendMessageServiceTest()
    {
        _target = new SendMessageService();
    }

    [Fact]
    public async Task Test_ExecuteAsync_シナリオ()
    {
        var messageId = $"{DateTime.Now:fffssmm}";
        var message   = new[] {new ChatworkMessage(982021, nameof(Test_ExecuteAsync_シナリオ))};

        var result = await _target.ExecuteAsync(message);

        result.Count().Is(1);
        result.First().Log.Is($"Send [{messageId}] {nameof(Test_ExecuteAsync_シナリオ)}");
    }
}