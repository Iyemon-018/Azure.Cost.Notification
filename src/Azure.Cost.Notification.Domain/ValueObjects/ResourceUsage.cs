namespace Azure.Cost.Notification.Domain.ValueObjects;

public sealed class ResourceUsage : ValueObjectBase<ResourceUsage>, IResourceUsage
{
    public ResourceUsage(decimal cost, string resourceGroupName, string serviceName, string id)
    {
        Cost              = cost;
        ResourceGroupName = resourceGroupName;
        ServiceName       = serviceName;
        Id                = id;
    }

    public decimal Cost { get; }

    public string ResourceGroupName { get; }

    public string ServiceName { get; }

    public string Id { get; }

    protected override bool EqualsCore(ResourceUsage other)
    {
        return Id == other.Id && Cost == other.Cost && ResourceGroupName == other.ResourceGroupName && ServiceName == other.ServiceName;
    }
}