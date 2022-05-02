namespace Azure.Cost.Notification.Application.Domain.Models;

public enum AggregationPeriodType
{
    LastDate,

    LastWeek,

    /// <summary>
    /// 現在の請求期間
    /// これは月ごとの請求期間のうち、請求金額が確定していない範囲を指しています。
    /// </summary>
    /// <remarks>
    /// Azure の場合、毎月23日から請求期間が開始される。
    /// 今日が4月28日とすると、4月23～27日までの情報が現在の請求期間となる。
    /// </remarks>
    CurrentBillingPeriod,
}