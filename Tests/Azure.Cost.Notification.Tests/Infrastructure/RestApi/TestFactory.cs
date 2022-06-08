namespace Azure.Cost.Notification.Tests.Infrastructure.RestApi;

using System.Net;
using Azure.RestApi.CostManagement;
using Azure.RestApi.CostManagement.Data;
using Moq;
using Repositories;

public sealed class TestFactory
{
    private readonly Mock<IClient> _client;

    private readonly Mock<ILogin> _login;

    public TestFactory()
    {
        _client = new Mock<IClient>();
        _login  = new Mock<ILogin>();

        _client.Setup(x => x.Login).Returns(LoginMock.Object);
        LoginMock.GetAccessTokenAsyncMock((tenantId, body, token) => AzureResponseBuilder.Error<AccessToken>(HttpStatusCode.Unauthorized));
    }

    public IClient Client => _client.Object;

    public Mock<ILogin> LoginMock => _login;
}