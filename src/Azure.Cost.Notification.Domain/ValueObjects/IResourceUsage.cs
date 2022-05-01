namespace Azure.Cost.Notification.Domain.ValueObjects;

public interface IResourceUsage
{
    decimal Cost { get; }

    string ResourceGroupName { get; }

    string ServiceName { get; }

    string Id { get; }
}