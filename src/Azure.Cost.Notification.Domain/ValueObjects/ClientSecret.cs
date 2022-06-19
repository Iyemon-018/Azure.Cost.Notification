namespace Azure.Cost.Notification.Domain.ValueObjects;

public sealed class ClientSecret : ValueObjectBase<ClientSecret>
{
    public ClientSecret(IReadOnlyDictionary<string, string> parameters)
            : this(parameters.TryGetValue("clientSecret", out var value) ? value : string.Empty)
    {

    }

    public ClientSecret(string value)
    {
        Value = value;
    }

    protected override bool EqualsCore(ClientSecret other)
    {
        return Value == other.Value;
    }

    public string Value { get; }
}