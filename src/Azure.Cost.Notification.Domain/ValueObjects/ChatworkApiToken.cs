namespace Azure.Cost.Notification.Domain.ValueObjects;

public sealed class ChatworkApiToken : ValueObjectBase<ChatworkApiToken>
{
    public ChatworkApiToken(IReadOnlyDictionary<string, string> parameters)
            : this(parameters.TryGetValue("chatworkApiToken", out var value) ? value : string.Empty)
    {

    }

    public ChatworkApiToken(string value)
    {
        Value = value;
    }

    protected override bool EqualsCore(ChatworkApiToken other)
    {
        return Value == other.Value;
    }

    public string Value { get; }
}