namespace Azure.Cost.Notification.Infrastructure.RestApi.Repositories;

using Azure.RestApi.CostManagement;
using Azure.RestApi.CostManagement.Data;
using Azure.RestApi.CostManagement.Requests;
using Domain.Repositories;
using Domain.ValueObjects;
using Extensions;

internal sealed class ResourceUsageRepository : IResourceUsageRepository
{
    private readonly IClient _client;

    public ResourceUsageRepository(IClient client)
    {
        _client = client;
    }

    private async Task<IEnumerable<ResourceUsage>> GetCostAsync(string subscriptionId, QueryUsageRequestBody body)
    {
        var usages    = new List<ResourceUsage>();
        var skipToken = string.Empty;

        while (true)
        {
            var response = await _client.Query.UsageAsync(QueryScope.Subscriptions(subscriptionId), body, skipToken: skipToken).ConfigureAwait(false);

            Assert(response);

            var usage = response.Content.AsResourceUsages().ToArray();
            usages.AddRange(usage);

            // nextlink に定義がなければ、取得できるデータはそこまでとなる。
            if (!response.Content.properties.HasNext) break;
            skipToken = response.Content.properties.SkipToken();
        }

        return usages;
    }

    public async Task<DailyCost> GetDailyCostAsync(string subscriptionId, DateTime target)
    {
        var usages = await GetCostAsync(subscriptionId, QueryUsageRequestBuilder.ToDaily(target)).ConfigureAwait(false);

        return new DailyCost(target, usages);
    }

    public async Task<WeeklyCost> GetWeeklyCostAsync(string subscriptionId, DateTime target)
    {
        var body   = QueryUsageRequestBuilder.ToWeekly(target);
        var usages = await GetCostAsync(subscriptionId, body).ConfigureAwait(false);

        return new WeeklyCost(body.timePeriod!.from, body.timePeriod.to, usages);
    }

    public async Task<MonthlyCost> GetMonthlyCostAsync(string subscriptionId)
    {
        var usages = await GetCostAsync(subscriptionId, QueryUsageRequestBuilder.ToBillingMonthly()).ConfigureAwait(false);

        return new MonthlyCost(usages);
    }

    private void Assert(AzureResponse<QueryResult> response)
    {
        if (!response.IsSuccess)
        {
            throw new AzureRestApiException(response.StatusCode
                  , response.RequestUri
                  , response.RequestMessage
                  , $"Failed get daily usage for Azure Cost Management REST API. {response.ErrorMessage}");
        }
    }
}