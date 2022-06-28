namespace Azure.Cost.Notification.Application.Domain.Models;

using Notification.Domain;
using Notification.Domain.Entities;
using Notification.Domain.ValueObjects;

/// <summary>
/// Azure REST API によって取得したリソース利用料金の結果を保持します。
/// </summary>
public sealed class TotalCostResult
{
    public TotalCostResult()
    {

    }

    public TotalCostResult(DailyCost dailyCost)
    {
        Usage      = dailyCost.Usage;
        PeriodType = AggregationPeriodType.LastDate;
        From       = dailyCost.Target;
        To         = dailyCost.Target;
    }

    public TotalCostResult(WeeklyCost weeklyCost)
    {
        Usage      = weeklyCost.Usage;
        PeriodType = AggregationPeriodType.LastWeek;
        From       = weeklyCost.PeriodFrom;
        To         = weeklyCost.PeriodTo;
    }

    public TotalCostResult(MonthlyCost monthlyCost)
    {
        Usage      = monthlyCost.Usage;
        PeriodType = AggregationPeriodType.CurrentBillingPeriod;
    }

    /// <summary>
    /// Azure REST API によって取得したリソースの利用料情報がすべてここに格納される。
    /// フィルタなどは原則子のクラスしか実施しないものとする。
    /// </summary>
    public IEnumerable<ResourceUsage> Usage { get; set; }

    /// <summary>
    /// 集計期間種別を返します。
    /// </summary>
    public AggregationPeriodType PeriodType { get; set; }

    /// <summary>
    /// 集計期間の開始日を取得します。
    /// </summary>
    public DateTime From { get; set; }

    /// <summary>
    /// 集計期間の終了日を取得します。
    /// </summary>
    public DateTime To { get; set; }

    /// <summary>
    /// 利用金額の高いリソースを取り出します。
    /// </summary>
    /// <param name="count">取り出す上位の数です。<para>5</para> の場合、金額の高い上位5つを返します。</param>
    /// <returns>取り出した要素を返します。</returns>
    public IEnumerable<ResourceUsage> TakeHighAmount(int count) => Usage.OrderByDescending(x => x.Cost).Take(count);

    /// <summary>
    /// すべてのリソースの合計金額を取得します。
    /// </summary>
    /// <returns>リソースの合計金額を返します。</returns>
    public decimal TotalCost() => Usage.Sum(x => x.Cost);

    /// <summary>
    /// すべてのリソースの合計金額を日本通貨の書式で取得します。
    /// </summary>
    /// <returns>リソースの合計金額を返します。</returns>
    public string TotalCostJapaneseCurrency() => TotalCost().ToString("C2", Constants.CultureJapanese);
}