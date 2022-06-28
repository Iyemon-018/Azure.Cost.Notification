namespace Azure.Cost.Notification.Domain.Entities;

using Models;

public sealed class ChatworkSendResult : Entity<string>
{
    public ChatworkSendResult(ChatworkMessage message, string messageId)
    {
        Message = message;
        Id      = messageId;
    }

    public ChatworkMessage Message { get; set; }


    public string Log() => $"Send:{Id}, Room:{Message.RoomId}, {Message}";
}