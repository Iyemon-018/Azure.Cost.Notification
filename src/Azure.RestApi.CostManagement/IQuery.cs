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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="scope"></param>
    /// <param name="body"></param>
    /// <param name="apiVersion"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <remarks>
    /// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage
    /// </remarks>
    Task<AzureResponse<QueryResult>> UsageAsync(QueryScope scope, QueryUsageRequestBody body, string? apiVersion = default, CancellationToken cancellationToken = default);
}