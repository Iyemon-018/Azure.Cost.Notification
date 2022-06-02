namespace Azure.Cost.Notification.Domain.ValueObjects;

public sealed class ChatworkSendResult : ValueObjectBase<ChatworkSendResult>
{
    public ChatworkSendResult(ChatworkMessage message, string messageId)
    {
        Log = $"Send:{messageId}, Room:{message.RoomId}, {message}";
    }

    public string Log { get; }

    protected override bool EqualsCore(ChatworkSendResult other)
    {
        return Log == other.Log;
    }
}