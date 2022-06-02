namespace Azure.RestApi.CostManagement.Data;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#queryresult
/// </remarks>
public sealed class QueryResult
{
    public string eTag { get; set; } = null!;

    public string id { get; set; } = null!;

    public string location { get; set; } = null!;

    public string name { get; set; } = null!;

    public string sku { get; set; } = null!;

    public string type { get; set; } = null!;

    public QueryResultProperty properties { get; set; } = null!;
}
