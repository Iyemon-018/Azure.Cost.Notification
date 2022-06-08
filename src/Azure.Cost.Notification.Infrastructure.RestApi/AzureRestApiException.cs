namespace Azure.Cost.Notification.Infrastructure.RestApi;

using System.Net;

[Serializable]
public sealed class AzureRestApiException : Exception
{
    private readonly HttpStatusCode _statusCode;

    private readonly Uri _uri;

    private readonly HttpRequestMessage _requestMessage;

    public AzureRestApiException(HttpStatusCode statusCode, Uri uri, HttpRequestMessage requestMessage, string message)
            : base(message)
    {
        _statusCode     = statusCode;
        _uri            = uri;
        _requestMessage = requestMessage;
    }
    
    public HttpStatusCode StatusCode => _statusCode;

    public Uri Uri => _uri;

    public HttpRequestMessage RequestMessage => _requestMessage;
}