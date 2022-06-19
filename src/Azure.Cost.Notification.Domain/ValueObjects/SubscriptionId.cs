namespace Azure.Cost.Notification.Domain.ValueObjects;

public sealed class SubscriptionId : ValueObjectBase<SubscriptionId>
{
    public SubscriptionId(IReadOnlyDictionary<string, string> parameters)
            : this(parameters.TryGetValue("subscriptionId", out var value) ? value : string.Empty)
    {

    }

    public SubscriptionId(string value)
    {
        Value = value;
    }

    protected override bool EqualsCore(SubscriptionId other)
    {
        return Value == other.Value;
    }

    public string Value { get; }
}