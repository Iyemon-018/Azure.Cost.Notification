namespace Azure.RestApi.CostManagement.Requests;

using Data;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#request-body
/// </remarks>
public sealed class QueryUsageRequest
{
    public QueryDataset dataset { get; set; }

    public TimeframeType timeframeType { get; set; }

    public ExportType type { get; set; }

    public QueryTimePeriod timePeriod { get; set; }
}