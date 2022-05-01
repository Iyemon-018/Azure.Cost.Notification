namespace Azure.Cost.Notification.Infrastructure.RestApi;

using Azure.RestApi.CostManagement;
using Domain.Repositories;
using Repositories;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly LoginRepository _loginRepository;

    private readonly ResourceUsageRepository _resourceUsageRepository;

    public UnitOfWork(HttpClient httpClient)
    {
        var client = new Client(httpClient);

        _loginRepository         = new LoginRepository(client);
        _resourceUsageRepository = new ResourceUsageRepository(client);
    }

    public ILoginRepository LoginRepository => _loginRepository;

    public IResourceUsageRepository ResourceUsageRepository => _resourceUsageRepository;
}