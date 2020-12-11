using System;
using Damascus.Domain.Abstractions;

namespace Damascus.Persistence.Abstractions.OptimisticLocking
{
    public abstract class VersionedAggregateRootBase<TIdentifier> : AggregateRootBase<TIdentifier>, IVersionedAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        protected VersionedAggregateRootBase(TIdentifier id) : base(id)
        {
            Version = 0;
        }

        public long Version { get; }
    }
}
