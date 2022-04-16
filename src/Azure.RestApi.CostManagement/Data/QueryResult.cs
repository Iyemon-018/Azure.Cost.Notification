namespace Azure.RestApi.CostManagement.Data;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#queryresult
/// </remarks>
public sealed class QueryResult
{
    public string eTag { get; set; }

    public string id { get; set; }

    public string location { get; set; }

    public string name { get; set; }

    public string sku { get; set; }

    public string type { get; set; }

    public QueryResultProperty properties { get; set; }
}
