namespace Azure.Cost.Notification.Domain.ValueObjects;

using Entities;

public sealed class DailyCost : ValueObjectBase<DailyCost>
{
    public DailyCost(DateTime target, IEnumerable<ResourceUsage> usage)
    {
        Target = target;
        Usage  = usage;
    }

    public DateTime Target { get; }

    public IEnumerable<ResourceUsage> Usage { get; }

    protected override bool EqualsCore(DailyCost other)
    {
        return Target == other.Target && Usage.Count() == other.Usage.Count() && Usage == other.Usage;
    }
}