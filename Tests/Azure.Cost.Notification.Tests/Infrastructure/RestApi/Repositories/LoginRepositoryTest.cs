namespace Azure.Cost.Notification.Tests.Infrastructure.RestApi.Repositories;

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Azure.RestApi.CostManagement;
using Azure.RestApi.CostManagement.Data;
using Azure.RestApi.CostManagement.Requests;
using ChainingAssertion;
using Moq;
using Notification.Domain.Repositories;
using Notification.Infrastructure.RestApi;
using Notification.Infrastructure.RestApi.Repositories;
using Xunit;

public class LoginRepositoryTest
{
    private readonly LoginRepository _target;

    private readonly TestFactory _testFactory;

    public LoginRepositoryTest()
    {
        _testFactory = new TestFactory();
        _target      = new LoginRepository(_testFactory.Client);
    }

    [Fact]
    public void Test_Authenticate_引数に設定した値がほしい位置まで引き継がれていること()
    {
        _testFactory.LoginMock.GetAccessTokenAsyncMock((tenantId, body, token) =>
        {
            // 引数に指定した値はそれぞれここに引き継がれる。
            tenantId.Is("tenantId");
            body.clientId.Is("clientId");
            body.clientSecret.Is("clientSecret");
            token.Is(CancellationToken.None);

            return AzureResponseBuilder.Error<AccessToken>(HttpStatusCode.Unauthorized);
        });

        var ex = Record.ExceptionAsync(() => _target.Authenticate("tenantId", "clientId", "clientSecret"));

        ex.Result.IsInstanceOf<AzureRestApiException>();
    }

    [Fact]
    public void Test_Authenticate_APIエラーになった場合のエラーメッセージが期待した値であること()
    {
        _testFactory.LoginMock.GetAccessTokenAsyncMock((_, _, _) => AzureResponseBuilder.Error<AccessToken>(HttpStatusCode.Unauthorized, requestUri: "https://www.google.co.jp/"));

        var ex = Record.ExceptionAsync(() => _target.Authenticate("tenantId", "clientId", "clientSecret"));

        ex.Result.IsInstanceOf<AzureRestApiException>();

        var apiEx = ex.Result as AzureRestApiException;

        apiEx.StatusCode.Is(HttpStatusCode.Unauthorized);
        apiEx.Uri.Is(x => x.AbsoluteUri == "https://www.google.co.jp/");
        apiEx.Message.Is("サービスプリンシパルの認証に失敗しました。");
    }

    [Fact]
    public void Test_Authenticate_APIエラーになったときAccessTokenが設定されないこと()
    {
        _testFactory.LoginMock.GetAccessTokenAsyncMock((_, _, _)
                => AzureResponseBuilder.Error<AccessToken>(HttpStatusCode.Unauthorized, requestUri: "https://www.google.co.jp/"));
        
        var ex = Record.ExceptionAsync(() => _target.Authenticate("tenantId", "clientId", "clientSecret"));

        ex.Result.IsInstanceOf<AzureRestApiException>();

        // 一度も AccessToken が呼ばれなければOK
        _testFactory.LoginMock.Verify(x => x.AccessToken(It.IsAny<AccessToken>()), Times.Never);
    }

    [Fact]
    public void Test_Authenticate_APIが成功したときAccessTokenが設定されること()
    {
        _testFactory.LoginMock.GetAccessTokenAsyncMock((_, _, _)
                => AzureResponseBuilder.Success(new AccessToken
                                                {
                                                    resource       = "resource"
                                                  , access_token   = "access_token"
                                                  , expires_in     = 10
                                                  , expires_on     = 20
                                                  , ext_expires_in = 1000
                                                  , not_before     = 2000
                                                  , token_type     = "token_type"
                                                }));

        _testFactory.LoginMock.AccessTokenMock(token =>
        {
            // API の実行結果がここで取得できればOK
            token.resource.Is("resource");
            token.access_token.Is("access_token");
            token.expires_in.Is(10);
            token.expires_on.Is(20);
            token.ext_expires_in.Is(1000);
            token.not_before.Is(2000);
            token.token_type.Is("token_type");
        });
        var ex = Record.ExceptionAsync(() => _target.Authenticate("tenantId", "clientId", "clientSecret"));

        ex.Result.IsNull();
    }

    [Fact]
    public async Task Test_Authenticate_APIが成功したとき応答結果の値が期待した値であること()
    {
        var today = DateTime.Today;
        _testFactory.LoginMock.GetAccessTokenAsyncMock((_, _, _)
                => AzureResponseBuilder.Success(new AccessToken
                                                {
                                                    resource       = "resource"
                                                  , access_token   = "access_token"
                                                  , expires_in     = 1892203
                                                  , expires_on     = 1892213
                                                  , ext_expires_in = 309007
                                                  , not_before     = 309217
                                                  , token_type     = "token_type"
                                                }));

        var response = await _target.Authenticate("tenantId", "clientId", "clientSecret");

        response.IsNotNull();
        response.AccessToken.Is("access_token");
        response.ExpiredOn.Is(DateTimeOffset.FromUnixTimeSeconds(1892213).LocalDateTime);
        response.NotBefore.Is(DateTimeOffset.FromUnixTimeSeconds(309217).LocalDateTime);
        response.TokenType.Is("token_type");
    }
}

public sealed class TestFactory
{
    private readonly Mock<IClient> _client;

    private readonly Mock<ILogin> _login;

    public TestFactory()
    {
        _client = new Mock<IClient>();
        _login = new Mock<ILogin>();

        _client.Setup(x => x.Login).Returns(LoginMock.Object);
        LoginMock.GetAccessTokenAsyncMock((tenantId, body, token) => AzureResponseBuilder.Error<AccessToken>(HttpStatusCode.Unauthorized));
    }

    public IClient Client => _client.Object;

    public Mock<ILogin> LoginMock => _login;
}

public static class LoginMockExtensions
{
    public static void GetAccessTokenAsyncMock(this Mock<ILogin> self, Func<string, AccessTokenRequestBody, CancellationToken, AzureResponse<AccessToken>> func)
        => self.Setup(x => x.GetAccessTokenAsync(It.IsAny<string>(), It.IsAny<AccessTokenRequestBody>(), It.IsAny<CancellationToken>())).ReturnsAsync(func);

    public static void AccessTokenMock(this Mock<ILogin> self, Action<AccessToken> action)
        => self.Setup(x => x.AccessToken(It.IsAny<AccessToken>())).Callback(action);
}

public static class AzureResponseBuilder
{
    public static AzureResponse<T> Success<T>(T              content
                                            , HttpStatusCode statusCode = HttpStatusCode.OK
                                            , HttpMethod?    method     = null
                                            , string?        requestUri = null) where T : class
        => new(content, statusCode, new HttpRequestMessage(method ?? HttpMethod.Get, requestUri));
    public static AzureResponse<T> Error<T>(HttpStatusCode statusCode
                                          , HttpMethod?    method     = null
                                          , string?        requestUri = null
                                          , string         code       = "1234"
                                          , string         message    = nameof(AzureResponseBuilder) + "." + nameof(Error)) where T : class
    {
        var error = new ErrorResponse
                    {
                        error = new ErrorDetails
                                {
                                    code = code, message = message
                                }
                    };
        return new(statusCode, new HttpRequestMessage(method ?? HttpMethod.Get, requestUri), error);
    }
}