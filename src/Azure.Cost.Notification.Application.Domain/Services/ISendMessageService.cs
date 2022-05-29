namespace Azure.Cost.Notification.Application.Domain.Services;

using Models;

public interface ISendMessageService
{
    Task<IEnumerable<ChatworkSendResult>> ExecuteAsync(IEnumerable<ChatworkMessage> message);
}

public sealed class SendMessageService : ISendMessageService
{
    public Task<IEnumerable<ChatworkSendResult>> ExecuteAsync(IEnumerable<ChatworkMessage> message)
    {
        throw new NotImplementedException();
    }
}