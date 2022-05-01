namespace Azure.Cost.Notification.Application.Domain.Models;

public class ChatworkMessage
{
    private readonly string _message;

    public ChatworkMessage(string message)
    {
        _message = message;
    }

    public override string ToString() => $"{_message}";
}