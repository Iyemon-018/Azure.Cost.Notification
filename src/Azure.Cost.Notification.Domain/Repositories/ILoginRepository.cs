namespace Azure.Cost.Notification.Domain.Repositories;

using ValueObjects;

public interface ILoginRepository
{
    Task<AzureAuthentication> Authenticate(string tenantId, string clientId, string clientSecret, CancellationToken cancellation = default);
}