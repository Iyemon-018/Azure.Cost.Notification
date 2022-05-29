namespace Azure.Cost.Notification.Application.Domain.Services;

using Notification.Domain.ValueObjects;

public interface ISendMessageService
{
    Task<IEnumerable<ChatworkSendResult>> ExecuteAsync(IEnumerable<ChatworkMessage> message);
}