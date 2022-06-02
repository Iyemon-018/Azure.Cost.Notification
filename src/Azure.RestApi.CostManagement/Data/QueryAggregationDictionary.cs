namespace Azure.RestApi.CostManagement.Data;

public sealed class QueryAggregationDictionary
{
    public QueryAggregation totalCost { get; set; } = null!;

    public static QueryAggregationDictionary Default() => new() {totalCost = QueryAggregation.Default()};
}