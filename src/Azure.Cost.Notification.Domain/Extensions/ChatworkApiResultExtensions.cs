namespace Azure.Cost.Notification.Domain.Extensions;

using Entities;

public static class ChatworkApiResultExtensions
{
    public static string Logs(this IEnumerable<ChatworkSendResult> self) => string.Join(Environment.NewLine, self.Select(x => x.Log()));
}