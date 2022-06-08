namespace Azure.Cost.Notification.Tests.Infrastructure.RestApi.Repositories;

using System.Net;
using System.Net.Http;
using Azure.RestApi.CostManagement;
using Azure.RestApi.CostManagement.Data;

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