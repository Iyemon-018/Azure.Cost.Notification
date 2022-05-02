namespace Azure.Cost.Notification.Domain.Repositories;

using ValueObjects;

public interface IResourceUsageRepository
{
    Task<DailyCost> GetDailyCostAsync(string subscriptionId, DateTime target);

    Task<WeeklyCost> GetWeeklyCostAsync(string subscriptionId, DateTime target);

    Task<MonthlyCost> GetMonthlyCostAsync(string subscriptionId);
}