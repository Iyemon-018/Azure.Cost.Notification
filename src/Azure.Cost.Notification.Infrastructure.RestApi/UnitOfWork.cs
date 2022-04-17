namespace Azure.Cost.Notification.Infrastructure.RestApi;

using Azure.RestApi.CostManagement;
using Domain.Repositories;
using Repositories;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly LoginRepository _loginRepository;

    public UnitOfWork(HttpClient httpClient)
    {
        var client = new Client(httpClient);
        _loginRepository = new LoginRepository(client);
    }

    public ILoginRepository LoginRepository => _loginRepository;
}