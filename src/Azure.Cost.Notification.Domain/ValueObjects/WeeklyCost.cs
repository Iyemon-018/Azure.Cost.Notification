namespace Azure.Cost.Notification.Domain.ValueObjects;

public sealed class WeeklyCost : ValueObjectBase<WeeklyCost>
{
    public WeeklyCost(DateTime periodFrom, DateTime periodTo, IEnumerable<ResourceUsage> usage)
    {
        PeriodFrom = periodFrom;
        PeriodTo   = periodTo;
        Usage      = usage;
    }

    public DateTime PeriodFrom { get; }

    public DateTime PeriodTo { get; }

    public IEnumerable<ResourceUsage> Usage { get; }

    protected override bool EqualsCore(WeeklyCost other)
    {
        return PeriodFrom == other.PeriodFrom && PeriodTo == other.PeriodTo && Usage.Count() == other.Usage.Count() && Usage == other.Usage;
    }
}