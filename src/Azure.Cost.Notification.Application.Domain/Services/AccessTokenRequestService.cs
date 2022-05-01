namespace Azure.Cost.Notification.Application.Domain.Services;

using Infrastructure.RestApi;
using Models;
using Notification.Domain.Repositories;
using Notification.Domain.ValueObjects;

public sealed class AccessTokenRequestService : IAccessTokenRequestService
{
    private readonly ILoginRepository _loginRepository;

    public AccessTokenRequestService(IUnitOfWork unitOfWork)
    {
        _loginRepository = unitOfWork.LoginRepository;
    }

    public async Task<AzureAuthentication> GetAsync(AzureAccessTokenRequest request)
    {
        return await _loginRepository.Authenticate(request.TenantId, request.ClientId, request.ClientSecret).ConfigureAwait(false);
    }
}