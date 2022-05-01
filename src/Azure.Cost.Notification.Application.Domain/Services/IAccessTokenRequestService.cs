namespace Azure.Cost.Notification.Application.Domain.Services;

using Models;
using Notification.Domain.ValueObjects;

public interface IAccessTokenRequestService
{
    public Task<AzureAuthentication> GetAsync(AzureAccessTokenRequest request);
}