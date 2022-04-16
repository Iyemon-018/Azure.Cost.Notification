namespace Azure.RestApi.CostManagement.Data;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#querydataset
/// </remarks>
public sealed class QueryDataset
{
    public QueryAggregationDictionary aggregation { get; set; }

    public QueryDatasetConfiguration configuration { get; set; }

    public QueryFilter filter { get; set; }

    public GranularityType granularity { get; set; }

    public QueryGrouping[] grouping { get; set; }
}