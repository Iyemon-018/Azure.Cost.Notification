namespace Azure.Cost.Notification.Application.Domain.Services;

using Infrastructure.ChatworkApi;
using Notification.Domain.Entities;
using Notification.Domain.Models;
using Notification.Domain.Repositories;

public sealed class SendMessageService : ISendMessageService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IMessageSendRepository _messageSendRepository;

    public SendMessageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork            = unitOfWork;
        _messageSendRepository = unitOfWork.MessageSendRepository;
    }

    public async IAsyncEnumerable<ChatworkSendResult> ExecuteAsync(string apiToken, IEnumerable<ChatworkMessage> message)
    {
        _unitOfWork.ApiToken(apiToken);

        foreach (var m in message)
        {
            yield return await _messageSendRepository.SendAsync(m).ConfigureAwait(false);
        }
    }
}