namespace Azure.Cost.Notification.Application.Domain.Services;

using Notification.Domain.ValueObjects;

public interface ISendMessageService
{
    IAsyncEnumerable<ChatworkSendResult> ExecuteAsync(IEnumerable<ChatworkMessage> message);
}