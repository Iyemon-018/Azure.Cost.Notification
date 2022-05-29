namespace Azure.Cost.Notification.Infrastructure.ChatworkApi;

using Domain.ValueObjects;
using global::ChatworkApi.Messages;

public static class ChatworkMessageBuilder
{
    public static ChatworkMessage Build(int roomId, string title, string message)
        => new(roomId, new MessageBuilder().Information.Add(title, message).Build());
}