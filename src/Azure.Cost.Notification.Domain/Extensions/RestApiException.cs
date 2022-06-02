namespace Azure.Cost.Notification.Domain.Extensions;

using System.Net;

public sealed class RestApiException : Exception
{
    public RestApiException(HttpStatusCode statusCode, IEnumerable<string> errors)
            : base($"StatusCode[{statusCode}]{Environment.NewLine}{string.Join(Environment.NewLine, errors.Select(x => $"  {x}"))}")
    {
    }
}