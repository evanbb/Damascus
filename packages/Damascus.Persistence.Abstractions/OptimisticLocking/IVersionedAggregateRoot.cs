using System;
using Damascus.Domain.Abstractions;

namespace Damascus.Persistence.Abstractions.OptimisticLocking
{
    public interface IVersionedAggregateRoot<TIdentifier>
        : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        long Version { get; }
    }
}
