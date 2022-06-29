namespace Azure.Cost.Notification.Infrastructure.ChatworkApi;

using Domain.Models;
using global::ChatworkApi.Messages;

public static class ChatworkMessageBuilder
{
    public static ChatworkMessage Build(int roomId, string title, string message)
        => new(roomId, new MessageBuilder().Information.Add(title, message).Add("※Azure.Cost.Notification による自動通知です。").Build());
}