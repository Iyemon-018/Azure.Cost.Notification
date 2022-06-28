namespace Azure.Cost.Notification.Application.Domain.Services;

using Notification.Domain.Entities;
using Notification.Domain.Models;

public interface ISendMessageService
{
    IAsyncEnumerable<ChatworkSendResult> ExecuteAsync(string apiToken, IEnumerable<ChatworkMessage> message);
}