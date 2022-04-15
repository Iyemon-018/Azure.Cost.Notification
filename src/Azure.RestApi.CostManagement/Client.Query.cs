namespace Azure.RestApi.CostManagement;

using Data;
using Requests;

public partial class Client : IQuery
{
    Task<QueryResult> IQuery.UsageAsync(string scope, QueryUsageRequest request, string? apiVersion = default)
    {
        throw new NotImplementedException();
    }
}