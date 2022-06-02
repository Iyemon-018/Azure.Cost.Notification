namespace Azure.RestApi.CostManagement.Data;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#querydatasetconfiguration
/// </remarks>
public sealed class QueryDatasetConfiguration
{
    public string[]? columns { get; set; }
}