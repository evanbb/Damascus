using System;
using Damascus.Core;

namespace Damascus.Domain.Abstractions
{
    public abstract class Entity<TIdentifier> : IEquatable<Entity<TIdentifier>>
        where TIdentifier : IEquatable<TIdentifier>
    {
        protected Entity(TIdentifier id)
        {
            Id = id;
        }

        public TIdentifier Id { get; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is Entity<TIdentifier> && Equals((Entity<TIdentifier>)obj);
        }

        public bool Equals(Entity<TIdentifier> other)
        {
            var thatType = other.GetType();
            var thisType = GetType();

            return thisType == thatType && Id.Equals(other.Id);
        }

        public static bool operator ==(Entity<TIdentifier> a, Entity<TIdentifier> b)
        {
            return !(a is null) && a.Equals(b);
        }

        public static bool operator !=(Entity<TIdentifier> a, Entity<TIdentifier> b)
        {
            return a is null || !a.Equals(b);
        }
    }

    // some aggregates have a single instance of an entity as a member
    // as there is only one instance of this entity inside the aggregate,
    // it will always have the same identifier
    public abstract class UnitEntity : Entity<Unit>
    {
        public UnitEntity() : base(new Unit())
        {

        }
    }
}
