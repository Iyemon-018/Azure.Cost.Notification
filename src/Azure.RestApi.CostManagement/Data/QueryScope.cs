namespace Azure.RestApi.CostManagement.Data;

public sealed class QueryScope
{
    public QueryScope(string scope)
    {
        _scope = scope;
    }

    private readonly string _scope;

    public override string ToString() => $"{_scope}";

    public static QueryScope Subscriptions(string subscriptionId) => new($"subscriptions/{subscriptionId}");
}