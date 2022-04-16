namespace Azure.RestApi.CostManagement;

using System.Text.Json.Serialization;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#exporttype
/// </remarks>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExportType
{
    ActualCost,
    AmortizedCost,
    Usage,
}