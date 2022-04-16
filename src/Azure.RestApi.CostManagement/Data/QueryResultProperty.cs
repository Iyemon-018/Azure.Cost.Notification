namespace Azure.RestApi.CostManagement.Data;

using System.Text.Json;

public sealed class QueryResultProperty
{
    public string nextLink { get; set; }

    public QueryColumn[] columns { get; set; }

    public JsonElement[][] rows { get; set; }
}