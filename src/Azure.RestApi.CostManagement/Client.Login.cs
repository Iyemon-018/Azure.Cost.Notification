namespace Azure.RestApi.CostManagement;

using System.Net;
using Data;
using Requests;

public partial class Client : ILogin
{
    async Task<AzureResponse<AccessToken>> ILogin.GetAccessTokenAsync(string tenantId, AccessTokenRequestBody body, CancellationToken cancellationToken = default)
    {
        var request  = RestApiRequest<AccessTokenRequestBody>.AsFormUrlEncodedContent($"https://login.microsoftonline.com/{tenantId}/oauth2/token", body);
        var response = await _tokenClient.PostAsync(request, cancellationToken: cancellationToken).ConfigureAwait(false);

        return await CreateResponseAsync<AccessToken>(response, HttpStatusCode.OK, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public void AccessToken(AccessToken token) => _tokenClient.AccessToken(token);
}