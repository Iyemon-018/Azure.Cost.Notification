namespace Azure.Cost.Notification.Infrastructure.ChatworkApi;

using Domain.Repositories;
using global::ChatworkApi;
using Repositories;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly IMessageSendRepository _messageSendRepository;

    public UnitOfWork(IClient client)
    {
        _messageSendRepository = new MessageSendRepository(client);
    }

    public IMessageSendRepository MessageSendRepository => _messageSendRepository;
}