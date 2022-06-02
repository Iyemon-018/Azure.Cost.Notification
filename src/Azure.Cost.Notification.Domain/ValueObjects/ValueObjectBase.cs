namespace Azure.Cost.Notification.Domain.ValueObjects;

public abstract class ValueObjectBase<T> where T : ValueObjectBase<T>
{
    protected bool Equals(ValueObjectBase<T> other)
    {
        return this == other;
    }

    // cf. https://docs.microsoft.com/ja-jp/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects#value-object-implementation-in-c
    protected virtual IEnumerable<object?> GetEqualityComponents()
        => typeof(T).GetProperties()
                    .Select(x => x.GetValue(this));

    public override int GetHashCode()
    {
        return GetEqualityComponents()
              .Select(x => x != null ? x.GetHashCode() : 0)
              .Aggregate((x, y) => x ^ y);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not T vo) return false;

        return EqualsCore(vo);
    }

    public static bool operator ==(ValueObjectBase<T> vo1, ValueObjectBase<T> vo2) => Equals(vo1, vo2);

    public static bool operator !=(ValueObjectBase<T> vo1, ValueObjectBase<T> vo2) => !Equals(vo1, vo2);

    protected abstract bool EqualsCore(T other);
}