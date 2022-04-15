namespace Azure.RestApi.CostManagement.Data;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#querytimeperiod
/// </remarks>
public sealed class QueryTimePeriod
{
    public DateTime from { get; set; }

    public DateTime to { get; set; }
}