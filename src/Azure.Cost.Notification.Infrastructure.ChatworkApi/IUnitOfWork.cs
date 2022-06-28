namespace Azure.Cost.Notification.Infrastructure.ChatworkApi;

using Domain.Repositories;

public interface IUnitOfWork
{
    IMessageSendRepository MessageSendRepository { get; }

    void ApiToken(string token);
}