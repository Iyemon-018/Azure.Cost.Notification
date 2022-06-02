namespace Azure.Cost.Notification.Domain.Repositories;

using ValueObjects;

public interface IMessageSendRepository
{
    Task<ChatworkSendResult> SendAsync(ChatworkMessage message, CancellationToken cancellationToken = default);
}
