namespace Azure.Cost.Notification.Domain.ValueObjects;

public sealed class RoomId : ValueObjectBase<RoomId>
{
    public RoomId(IReadOnlyDictionary<string, string> parameters)
            : this(parameters.TryGetValue("roomId", out var value) ? int.Parse(value) : 0)
    {

    }

    public RoomId(int value)
    {
        Value = value;
    }

    protected override bool EqualsCore(RoomId other)
    {
        return Value == other.Value;
    }

    public int Value { get; }
}