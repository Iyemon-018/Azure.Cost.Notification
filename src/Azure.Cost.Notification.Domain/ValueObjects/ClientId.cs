namespace Azure.Cost.Notification.Domain.ValueObjects;

public sealed class ClientId : ValueObjectBase<ClientId>
{
    public ClientId(IReadOnlyDictionary<string, string> parameters)
            : this(parameters.TryGetValue("clientId", out var value) ? value : string.Empty)
    {

    }

    public ClientId(string value)
    {
        Value = value;
    }

    protected override bool EqualsCore(ClientId other)
    {
        return Value == other.Value;
    }

    public string Value { get; }
}