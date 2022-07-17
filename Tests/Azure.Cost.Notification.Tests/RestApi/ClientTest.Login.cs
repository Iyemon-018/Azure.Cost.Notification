namespace Azure.Cost.Notification.Tests.RestApi;

using System.Net;
using System.Threading.Tasks;
using Azure.RestApi.CostManagement.Requests;
using ChainingAssertion;
using Moq.Contrib.HttpClient;
using Xunit;

public partial class ClientTest
{
    [Fact]
    public async Task Test_Login_GetAccessTokenAsync_200OKを返す場合に応答結果の値がレスポンスと一致すること()
    {
        _testFactory.Handler
                    .SetupAnyRequest()
                    .ReturnsResponse("{\"token_type\": \"Bearer\""
                                   + ",\"expires_in\": \"3599\""
                                   + ",\"ext_expires_in\": \"1599\""
                                   + ",\"expires_on\": \"1656516529\""
                                   + ",\"not_before\": \"1656512629\""
                                   + ",\"resource\": \"https://management.core.windows.net/\""
                                   + ",\"access_token\": \"<access_token>\"}"
                           , "application/json");

        var response = await _target.Login
                                    .GetAccessTokenAsync("0000-0000-0000", AccessTokenRequestBody.AsClientCredentials("user-id", "xxxxx"));

        response.IsSuccess.IsTrue();
        response.StatusCode.Is(HttpStatusCode.OK);

        response.Content.token_type.Is("Bearer");
        response.Content.expires_in.Is(3599);
        response.Content.ext_expires_in.Is(1599);
        response.Content.expires_on.Is(1656516529);
        response.Content.not_before.Is(1656512629);
        response.Content.resource.Is("https://management.core.windows.net/");
        response.Content.access_token.Is("<access_token>");

        response.ErrorMessage.IsNull();
    }

    [Fact]
    public async Task Test_Login_GetAccessTokenAsync_応答がエラーの場合にエラー情報を返すこと()
    {
        _testFactory.SetupErrorResponse();

        var response = await _target.Login
                                    .GetAccessTokenAsync("0000-0000-0000", AccessTokenRequestBody.AsClientCredentials("user-id", "xxxxxx"));

        response.IsSuccess.IsFalse();
        response.StatusCode.Is(HttpStatusCode.Unauthorized);
        response.ErrorMessage.Is("AuthenticationFailed: Authentication failed.");

        response.Content.IsNull();
        response.RequestUri.AbsoluteUri.Is("https://login.microsoftonline.com/0000-0000-0000/oauth2/token");
    }
}