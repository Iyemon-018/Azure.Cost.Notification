namespace Azure.Cost.Notification.Application.Domain.Services;

using Notification.Domain.ValueObjects;

public sealed class SendMessageService : ISendMessageService
{
    public Task<IEnumerable<ChatworkSendResult>> ExecuteAsync(IEnumerable<ChatworkMessage> message)
    {
        throw new NotImplementedException();
    }
}