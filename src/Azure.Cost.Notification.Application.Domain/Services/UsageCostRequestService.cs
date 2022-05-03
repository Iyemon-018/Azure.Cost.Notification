namespace Azure.Cost.Notification.Application.Domain.Services;

using Infrastructure.RestApi;
using Notification.Domain.Repositories;
using Notification.Domain.ValueObjects;

public sealed class UsageCostRequestService : IUsageCostRequestService
{
    private readonly IResourceUsageRepository _resourceUsageRepository;

    public UsageCostRequestService(IUnitOfWork unitOfWork)
    {
        _resourceUsageRepository = unitOfWork.ResourceUsageRepository;
    }

    public async Task<DailyCost> GetDailyCostAsync(string subscriptionId)
        => await _resourceUsageRepository.GetDailyCostAsync(subscriptionId: subscriptionId
                                                , DateTime.UtcNow.Date.AddDays(-1))
                                         .ConfigureAwait(false);

    public async Task<WeeklyCost> GetWeeklyCostAsync(string subscriptionId)
        => await _resourceUsageRepository.GetWeeklyCostAsync(subscriptionId
                                                , DateTime.UtcNow.Date.AddDays(-1))
                                         .ConfigureAwait(false);

    public async Task<MonthlyCost> GetMonthlyCostAsync(string subscriptionId)
        => await _resourceUsageRepository.GetMonthlyCostAsync(subscriptionId).ConfigureAwait(false);
}