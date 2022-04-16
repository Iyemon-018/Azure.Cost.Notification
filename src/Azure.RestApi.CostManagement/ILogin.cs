namespace Azure.RestApi.CostManagement;

using Data;
using Requests;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/azure/databricks/dev-tools/api/latest/aad/service-prin-aad-token
/// cf. https://docs.microsoft.com/ja-jp/azure/active-directory/verifiable-credentials/get-started-request-api?tabs=http
/// </remarks>
public interface ILogin
{
    Task<AzureResponse<AccessToken>> GetAccessTokenAsync(string tenantId, AccessTokenRequestBody body, CancellationToken cancellationToken = default);

    void AccessToken(AccessToken token);
}