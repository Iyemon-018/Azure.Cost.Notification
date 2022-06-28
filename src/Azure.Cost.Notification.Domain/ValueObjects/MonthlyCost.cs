namespace Azure.Cost.Notification.Domain.ValueObjects;

using Entities;

public sealed class MonthlyCost : ValueObjectBase<MonthlyCost>
{
    public MonthlyCost(IEnumerable<ResourceUsage> usage)
    {
        Usage = usage;
    }
    
    public IEnumerable<ResourceUsage> Usage { get; }

    protected override bool EqualsCore(MonthlyCost other)
    {
        return Usage.Count() == other.Usage.Count() && Usage == other.Usage;
    }
}