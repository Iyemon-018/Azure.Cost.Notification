namespace Azure.Cost.Notification.Tests.Infrastructure.RestApi.Repositories;

using System;
using System.Threading;
using Azure.RestApi.CostManagement;
using Azure.RestApi.CostManagement.Data;
using Azure.RestApi.CostManagement.Requests;
using Moq;

public static class LoginMockExtensions
{
    public static void GetAccessTokenAsyncMock(this Mock<ILogin> self, Func<string, AccessTokenRequestBody, CancellationToken, AzureResponse<AccessToken>> func)
        => self.Setup(x => x.GetAccessTokenAsync(It.IsAny<string>(), It.IsAny<AccessTokenRequestBody>(), It.IsAny<CancellationToken>())).ReturnsAsync(func);

    public static void AccessTokenMock(this Mock<ILogin> self, Action<AccessToken> action)
        => self.Setup(x => x.AccessToken(It.IsAny<AccessToken>())).Callback(action);
}