namespace Azure.Cost.Notification.Domain.Entities;

/// <summary>
/// すべての Entity の機能を有するクラスです。
/// </summary>
/// <remarks>
/// cf. https://enterprisecraftsmanship.com/posts/entity-base-class/
/// </remarks>
public abstract class Entity<T> where T : notnull
{
    public virtual T Id { get; set; }

    protected Entity()
    {

    }

    protected Entity(T id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<T> other) return false;

        if (ReferenceEquals(this, other)) return true;

        // Id が null というのは == default であると想定する。
        // default の値同士は初期化前であることを示すため、不一致であると判断する。
        if (Id is null || other.Id is null) return false;

        return Id.Equals(other.Id);
    }

    public static bool operator ==(Entity<T>? a, Entity<T>? b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity<T>? a, Entity<T>? b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}