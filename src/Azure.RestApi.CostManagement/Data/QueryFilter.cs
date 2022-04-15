namespace Azure.RestApi.CostManagement.Data;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#queryfilter
/// </remarks>
public sealed class QueryFilter
{
    public QueryFilter[] and { get; set; }

    public QueryComparisonExpression dimensions { get; set; }

    public QueryFilter[] or { get; set; }

    public QueryComparisonExpression tags { get; set; }
}