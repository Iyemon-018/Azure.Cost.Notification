namespace Azure.Cost.Notification.Infrastructure.ChatworkApi;

using Domain.Repositories;
using global::ChatworkApi;
using Repositories;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly IClient _client;

    public UnitOfWork(IClient client)
    {
        _client               = client;
        MessageSendRepository = new MessageSendRepository(client);
    }

    public IMessageSendRepository MessageSendRepository { get; }

    public void ApiToken(string token) => _client.ApiToken(token);
}