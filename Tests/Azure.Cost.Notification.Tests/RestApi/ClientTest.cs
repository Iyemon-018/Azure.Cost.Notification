namespace Azure.Cost.Notification.Tests.RestApi;

using System.Net;
using System.Net.Http;
using Azure.RestApi.CostManagement;
using Moq;
using Moq.Contrib.HttpClient;

public partial class ClientTest
{
    private readonly TestFactory _testFactory;

    private readonly Client _target;


    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// cf. https://github.com/maxkagamine/Moq.Contrib.HttpClient/blob/master/README.ja.md
    /// </remarks>
    public class TestFactory
    {
        public TestFactory()
        {
            Handler = new Mock<HttpMessageHandler>();
        }

        public Mock<HttpMessageHandler> Handler { get; }

        public HttpClient CreateClient() => Handler.CreateClient();

        public void SetupErrorResponse(HttpStatusCode statusCode = HttpStatusCode.Unauthorized
                                     , string         code       = "AuthenticationFailed"
                                     , string         message    = "Authentication failed.")
            => Handler
              .SetupAnyRequest()
              .ReturnsResponse(HttpStatusCode.Unauthorized, $"{{ \"error\": {{\"code\": \"{code}\", \"message\": \"{message}\"}}}}");
    }

    public ClientTest()
    {
        _testFactory = new TestFactory();
        _target      = new Client(_testFactory.CreateClient());
    }
}