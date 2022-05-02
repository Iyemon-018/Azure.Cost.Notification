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

    // TODO ここに Weekly も Monthly も追加する。
}