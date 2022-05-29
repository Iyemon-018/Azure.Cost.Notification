namespace Azure.Cost.Notification.Domain.ValueObjects;

public sealed class ChatworkMessage : ValueObjectBase<ChatworkMessage>
{
    private readonly int _roomId;

    private readonly string _message;

    public ChatworkMessage(int roomId, string message)
    {
        _roomId  = roomId;
        _message = message;
    }

    public int RoomId => _roomId;

    public string Message => _message;

    public override string ToString() => $"{Message}";

    protected override bool EqualsCore(ChatworkMessage other)
    {
        return RoomId == other.RoomId && Message == other.Message;
    }
}

public sealed class ChatworkSendResult : ValueObjectBase<ChatworkSendResult>
{
    public ChatworkSendResult(ChatworkMessage message, string messageId)
    {
        Log = $"Send [{messageId}] {message}";
    }

    public string Log { get; }

    protected override bool EqualsCore(ChatworkSendResult other)
    {
        return Log == other.Log;
    }
}

public static class ChatworkApiResultExtensions
{
    public static string Logs(this IEnumerable<ChatworkSendResult> self) => string.Join(Environment.NewLine, self.Select(x => x.Log));
}