namespace Azure.Cost.Notification.Application.Domain.Models;

public class ChatworkSendResult
{
    public ChatworkSendResult(ChatworkMessage message, string messageId)
    {
        Log = $"Send [{messageId}] {message}";
    }

    public string Log { get; }
}