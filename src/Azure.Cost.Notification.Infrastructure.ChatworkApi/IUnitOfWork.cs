namespace Azure.Cost.Notification.Infrastructure.ChatworkApi
{
    public interface IUnitOfWork
    {
        IMessageSendRepository MessageSendRepository { get; }
    }
}