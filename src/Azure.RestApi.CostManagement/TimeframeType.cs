namespace Azure.RestApi.CostManagement;

using System.Text.Json.Serialization;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#timeframetype
/// </remarks>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TimeframeType
{
    BillingMonthToDate,
    Custom,
    MonthToDate,
    TheLastBillingMonth,
    TheLastMonth,
    WeekToDate,
}