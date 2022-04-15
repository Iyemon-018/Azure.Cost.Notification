namespace Azure.RestApi.CostManagement;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#timeframetype
/// </remarks>
public enum TimeframeType
{
    BillingMonthToDate,
    Custom,
    MonthToDate,
    TheLastBillingMonth,
    TheLastMonth,
    WeekToDate,
}