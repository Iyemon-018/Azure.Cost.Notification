namespace Azure.Cost.Notification.Domain.Repositories;

using Entities;
using Models;

public interface IMessageSendRepository
{
    Task<ChatworkSendResult> SendAsync(ChatworkMessage message, CancellationToken cancellationToken = default);
}
