namespace Azure.RestApi.CostManagement.Requests;

using Data;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#request-body
/// </remarks>
public sealed class QueryUsageRequestBody
{
    public QueryDataset? dataset { get; set; }

    public TimeframeType timeframe { get; set; }

    public ExportType type { get; set; }

    public QueryTimePeriod? timePeriod { get; set; }
}