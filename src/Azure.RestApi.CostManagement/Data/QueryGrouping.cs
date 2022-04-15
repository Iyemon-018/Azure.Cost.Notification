namespace Azure.RestApi.CostManagement.Data;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#querygrouping
/// </remarks>
public sealed class QueryGrouping
{
    public string name { get; set; }

    public QueryColumnType type { get; set; }
}