namespace Azure.Cost.Notification.Infrastructure.ChatworkApi.Repositories;

using Domain.Extensions;
using Domain.Repositories;
using Domain.ValueObjects;
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

        if (!result.IsSuccess) throw new RestApiException(result.StatusCode, result.Errors());

        return new ChatworkSendResult(message, result.Content.message_id);
    }
}