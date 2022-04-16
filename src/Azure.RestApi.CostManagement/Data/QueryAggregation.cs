namespace Azure.RestApi.CostManagement.Data;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#queryaggregation
/// </remarks>
public sealed class QueryAggregation
{
    public FunctionType function { get; set; }

    public string name { get; set; }

    public static QueryAggregation Default() => new() {function = FunctionType.Sum, name = "PreTaxCost"};
}