namespace Azure.Cost.Notification.Domain.Repositories;

using ValueObjects;

public interface IResourceUsageRepository
{
    Task<DailyCost> GetDailyCostAsync(string subscriptionId, DateTime target);
}