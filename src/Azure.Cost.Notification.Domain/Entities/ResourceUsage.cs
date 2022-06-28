namespace Azure.Cost.Notification.Domain.Entities;

public sealed class ResourceUsage : Entity<string>, IResourceUsage
{
    public ResourceUsage(decimal cost, string resourceGroupName, string serviceName, string id)
    {
        Cost              = cost;
        ResourceGroupName = resourceGroupName;
        ServiceName       = serviceName;
        Id                = id;
    }

    public decimal Cost { get; set; }

    public string ResourceGroupName { get; set; }

    public string ServiceName { get; set; }

    public string CostJapaneseCurrency() => Cost.ToString("C2", Constants.CultureJapanese);
}