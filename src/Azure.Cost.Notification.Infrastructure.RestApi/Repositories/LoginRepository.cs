namespace Azure.Cost.Notification.Infrastructure.RestApi.Repositories;

using Azure.RestApi.CostManagement;
using Azure.RestApi.CostManagement.Requests;
using Domain.Repositories;
using Domain.ValueObjects;

internal class LoginRepository : ILoginRepository
{
    private readonly IClient _client;

    public LoginRepository(IClient client)
    {
        _client = client;
    }

    public async Task<AzureAuthentication> Authenticate(string tenantId, string clientId, string clientSecret, CancellationToken cancellation = default)
    {
        var body     = AccessTokenRequestBody.AsClientCredentials(clientId, clientSecret);
        var response = await _client.Login.GetAccessTokenAsync(tenantId, body, cancellation).ConfigureAwait(false);

        if (!response.IsSuccess)
            throw new AzureRestApiException(response.StatusCode, response.RequestUri, response.RequestMessage, $"サービスプリンシパルの認証に失敗しました。");

        var content = response.Content;

        _client.Login.AccessToken(content);

        return new AzureAuthentication(content.expires_on, content.not_before, content.token_type, content.access_token);
    }
}