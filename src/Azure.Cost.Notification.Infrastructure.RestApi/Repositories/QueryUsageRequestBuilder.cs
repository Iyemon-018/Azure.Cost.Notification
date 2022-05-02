namespace Azure.Cost.Notification.Infrastructure.RestApi.Repositories;

using Azure.RestApi.CostManagement;
using Azure.RestApi.CostManagement.Data;
using Azure.RestApi.CostManagement.Requests;

internal static class QueryUsageRequestBuilder
{
    private static readonly QueryGrouping[] DefaultGroup
            =
            {
                QueryGrouping.ResourceGroupName()
              , QueryGrouping.ServiceName()
              , QueryGrouping.ResourceId()
            };

    public static QueryUsageRequestBody ToDaily(DateTime target)
        => new()
           {
               type      = ExportType.Usage
             , timeframe = TimeframeType.Custom
             , timePeriod = new QueryTimePeriod
                            {
                                @from = target, to = target
                            }
             , dataset = new QueryDataset
                         {
                             aggregation = QueryAggregationDictionary.Default()
                           , granularity = GranularityType.Daily
                           , grouping    = DefaultGroup
                         }
           };

    public static QueryUsageRequestBody ToWeekly(DateTime target)
        => new()
           {
               type      = ExportType.Usage
             , timeframe = TimeframeType.Custom
             , timePeriod = new QueryTimePeriod
                            {
                                @from = target.AddDays(-6), to = target
                            }
             , dataset = new QueryDataset
                         {
                             aggregation = QueryAggregationDictionary.Default()
                           , granularity = GranularityType.Weekly
                           , grouping    = DefaultGroup
                         }
           };

    public static QueryUsageRequestBody ToBillingMonthly()
        => new()
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
}