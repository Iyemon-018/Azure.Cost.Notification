namespace Azure.Cost.Notification.Application.Domain.Services;

using Models;

internal static class MessageBuildExtensions
{
    public static string AsTitle(this TotalCostResult self)
    {
        switch (self.PeriodType)
        {
            case AggregationPeriodType.LastDate:             return $"{self.To:yyyy/MM/dd} の利用料金(Daily)";
            case AggregationPeriodType.LastWeek:             return $"{self.From:yyyy/MM/dd} - {self.To:yyyy/MM/dd} の利用料金(Weekly)";
            case AggregationPeriodType.CurrentBillingPeriod: return $"今月の利用料金(Monthly)";
            default:
                throw new NotSupportedException($"{typeof(AggregationPeriodType)}.{self.PeriodType} の値はサポートされていません。");
        }
    }

    public static string AsResourcesCost(this TotalCostResult self)
        => string.Join(Environment.NewLine, self.TakeHighAmount(5).Select(x => $"- {x.ResourceGroupName} / {x.Id}({x.ServiceName}) {x.Cost:C2}").ToArray());
}