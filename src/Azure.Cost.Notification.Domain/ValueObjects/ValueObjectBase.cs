namespace Azure.Cost.Notification.Domain.ValueObjects;

public abstract class ValueObjectBase<T> where T : ValueObjectBase<T>
{
    public override bool Equals(object? obj)
    {
        if (obj is not T vo) return false;

        return EqualsCore(vo);
    }

    public static bool operator ==(ValueObjectBase<T> vo1, ValueObjectBase<T> vo2) => Equals(vo1, vo2);

    public static bool operator !=(ValueObjectBase<T> vo1, ValueObjectBase<T> vo2) => !Equals(vo1, vo2);

    protected abstract bool EqualsCore(T other);
}