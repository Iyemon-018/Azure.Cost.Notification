namespace Azure.Cost.Notification.Application.Domain.Services;

using Notification.Domain.ValueObjects;

public interface IUsageCostRequestService
{
    Task<DailyCost> GetDailyCostAsync(string subscriptionId);
}