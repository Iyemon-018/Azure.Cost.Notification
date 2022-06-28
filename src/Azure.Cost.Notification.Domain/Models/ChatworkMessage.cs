namespace Azure.Cost.Notification.Domain.Models;

public sealed class ChatworkMessage
{
    public ChatworkMessage()
    {

    }

    public ChatworkMessage(int roomId, string message)
    {
        RoomId  = roomId;
        Message = message;
    }

    public int RoomId { get; set; }

    public string Message { get; set; }

    public override string ToString() => $"{Message}";
}