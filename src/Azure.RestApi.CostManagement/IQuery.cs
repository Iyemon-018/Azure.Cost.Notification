namespace Azure.RestApi.CostManagement;

using Data;
using Requests;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query
/// </remarks>
public interface IQuery
{
    Task<QueryResult> UsageAsync(string scope, QueryUsageRequest request, string? apiVersion = default);
}