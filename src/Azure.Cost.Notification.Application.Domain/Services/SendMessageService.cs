namespace Azure.Cost.Notification.Application.Domain.Services;

using Infrastructure.ChatworkApi;
using Notification.Domain.ValueObjects;

public sealed class SendMessageService : ISendMessageService
{
    private readonly IMessageSendRepository _messageSendRepository;

    public SendMessageService(IUnitOfWork unitOfWork)
    {
        _messageSendRepository = unitOfWork.MessageSendRepository;
    }

    public async IAsyncEnumerable<ChatworkSendResult> ExecuteAsync(IEnumerable<ChatworkMessage> message)
    {
        foreach (var m in message)
        {
            yield return await _messageSendRepository.SendAsync(m).ConfigureAwait(false);
        }
    }
}