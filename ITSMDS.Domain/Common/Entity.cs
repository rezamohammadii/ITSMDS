

// Entity.cs
namespace ITSMDS.Domain.Common;
    public abstract class Entity<TId>
    {
        public TId Id { get; protected set; }

        protected Entity() { }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity<TId> other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            if (Id.Equals(default) || other.Id.Equals(default))
                return false;

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id).GetHashCode();
        }

        public static bool operator ==(Entity<TId>? a, Entity<TId>? b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity<TId>? a, Entity<TId>? b)
        {
            return !(a == b);
        }
    }
    public abstract class Entity : Entity<int> { }
