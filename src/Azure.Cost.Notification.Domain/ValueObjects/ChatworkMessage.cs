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