namespace Azure.Cost.Notification.Infrastructure.ChatworkApi;

using System.Net;
using Domain.ValueObjects;

// TODO これは後で Domain.Repositories に移動させる。
public interface IMessageSendRepository
{
    Task<ChatworkSendResult> SendAsync(ChatworkMessage message, CancellationToken cancellationToken = default);
}

// TODO これは後で Domain.Exceptions に移動する。
public sealed class RestApiException : Exception
{
    public RestApiException(HttpStatusCode statusCode, IEnumerable<string> errors)
    : base($"StatusCode[{statusCode}]{Environment.NewLine}{string.Join(Environment.NewLine, errors.Select(x => $"  {x}"))}")
    {
    }
}