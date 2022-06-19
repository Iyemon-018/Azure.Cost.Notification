namespace Azure.Cost.Notification.Domain.ValueObjects;

public sealed class TenantId : ValueObjectBase<TenantId>
{
    public TenantId(IReadOnlyDictionary<string, string> parameters)
            : this(parameters.TryGetValue("tenantId", out var value) ? value : string.Empty)
    {

    }

    public TenantId(string value)
    {
        Value = value;
    }

    protected override bool EqualsCore(TenantId other)
    {
        return Value == other.Value;
    }

    public string Value { get; }
}