namespace Azure.Cost.Notification.Infrastructure.RestApi.Repositories;

using Azure.RestApi.CostManagement;
using Azure.RestApi.CostManagement.Data;
using Azure.RestApi.CostManagement.Requests;
using Domain.Repositories;
using Domain.ValueObjects;
using Extensions;

internal sealed class ResourceUsageRepository : IResourceUsageRepository
{
    private static readonly QueryGrouping[] DefaultGroup
            =
            {
                QueryGrouping.ResourceGroupName()
              , QueryGrouping.ServiceName()
              , QueryGrouping.ResourceId()
            };

    private readonly IClient _client;

    public ResourceUsageRepository(IClient client)
    {
        _client = client;
    }

    public async Task<DailyCost> GetDailyCostAsync(string subscriptionId, DateTime target)
    {
        var body = new QueryUsageRequestBody
                   {
                       type      = ExportType.Usage
                     , timeframe = TimeframeType.Custom
                     , timePeriod = new QueryTimePeriod
                                    {
                                        from = target, to = target
                                    }
                     , dataset = new QueryDataset
                                 {
                                     aggregation = QueryAggregationDictionary.Default()
                                   , granularity = GranularityType.Daily
                                   , grouping    = DefaultGroup
                                 }
                   };
        var response = await _client.Query.UsageAsync(QueryScope.Subscriptions(subscriptionId), body).ConfigureAwait(false);

        Assert(response);

        var usage = response.Content.AsResourceUsages().ToArray();
        return new DailyCost(target, usage);
    }

    public async Task<WeeklyCost> GetWeeklyCostAsync(string subscriptionId, DateTime target)
    {
        var body = new QueryUsageRequestBody
                   {
                       type      = ExportType.Usage
                     , timeframe = TimeframeType.Custom
                     , timePeriod = new QueryTimePeriod
                                    {
                                        from = target.AddDays(-7), to = target
                                    }
                     , dataset = new QueryDataset
                                 {
                                     aggregation = QueryAggregationDictionary.Default()
                                   , granularity = GranularityType.Weekly
                                   , grouping    = DefaultGroup
                                 }
                   };
        var response = await _client.Query.UsageAsync(QueryScope.Subscriptions(subscriptionId), body).ConfigureAwait(false);

        Assert(response);

        var usage = response.Content.AsResourceUsages().ToArray();
        return new WeeklyCost(body.timePeriod.from, body.timePeriod.to, usage);
    }

    public async Task<MonthlyCost> GetMonthlyCostAsync(string subscriptionId)
    {
        var body = new QueryUsageRequestBody
                   {
                       type      = ExportType.Usage
                     , timeframe = TimeframeType.BillingMonthToDate
                     , dataset = new QueryDataset
                                 {
                                     aggregation = QueryAggregationDictionary.Default()
                                   , granularity = GranularityType.Monthly
                                   , grouping    = DefaultGroup
                                 }
                   };
        var response = await _client.Query.UsageAsync(QueryScope.Subscriptions(subscriptionId), body).ConfigureAwait(false);

        Assert(response);

        var usage = response.Content.AsResourceUsages().ToArray();
        return new MonthlyCost(usage);
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