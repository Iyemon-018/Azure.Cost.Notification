namespace Azure.Cost.Notification.Infrastructure.ChatworkApi.Repositories;

using Domain.Entities;
using Domain.Extensions;
using Domain.Models;
using Domain.Repositories;
using global::ChatworkApi;

internal sealed class MessageSendRepository : IMessageSendRepository
{
    private readonly IClient _client;
    
    public MessageSendRepository(IClient client)
    {
        _client = client;
    }

    public async Task<ChatworkSendResult> SendAsync(ChatworkMessage message, CancellationToken cancellationToken = default)
    {
        var result = await _client.Rooms.AddMessageAsync(message.RoomId, message.Message, false, cancellationToken).ConfigureAwait(false);

        if (!result.IsSuccess) throw new RestApiException(result.StatusCode, result.Errors() ?? Enumerable.Empty<string>());

        return new ChatworkSendResult(message, result.Content.message_id);
    }
}