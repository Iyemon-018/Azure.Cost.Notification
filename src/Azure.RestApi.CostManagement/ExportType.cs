namespace Azure.RestApi.CostManagement;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#exporttype
/// </remarks>
public enum ExportType
{
    ActualCost,
    AmortizedCost,
    Usage,
}