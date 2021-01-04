using System;

namespace Damascus.Domain.Abstractions
{
    public interface IVersionedAggregateRoot<TIdentifier>
        : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        long Version { get; }
    }
}
