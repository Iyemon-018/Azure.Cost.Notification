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

    public static QueryGrouping ResourceGroupName() => new() {name = "ResourceGroupName", type = QueryColumnType.Dimension};

    public static QueryGrouping ServiceName() => new() {name = "ServiceName", type = QueryColumnType.Dimension};

    public static QueryGrouping ServiceTier() => new() {name = "ServiceTier", type = QueryColumnType.Dimension};

    public static QueryGrouping ResourceType() => new() {name = "ResourceType", type = QueryColumnType.Dimension};

    public static QueryGrouping ResourceId() => new() {name = "ResourceId", type = QueryColumnType.Dimension};
}