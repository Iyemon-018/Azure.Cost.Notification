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
            = {
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
                       type          = ExportType.Usage
                     , timeframeType = TimeframeType.Custom
                     , dataset = new QueryDataset
                                 {
                                     aggregation = QueryAggregationDictionary.Default()
                                   , granularity = GranularityType.Daily
                                   , grouping    = DefaultGroup
                                 }
                   };
        var response = await _client.Query.UsageAsync(QueryScope.Subscriptions(subscriptionId), body).ConfigureAwait(false);

        return response.IsSuccess
                ? response.Content.AsDailyCost(target)
                : throw new AzureRestApiException(response.StatusCode, response.RequestUri, response.RequestMessage, $"Failed get daily usage for Azure Cost Management REST API.");
    }
}