using System;

namespace Damascus.Domain.Abstractions.Events
{
    public abstract class PersistedDomainEventBase<TIdentifier> : IPersistedDomainEvent<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        protected PersistedDomainEventBase(TIdentifier aggregateId)
        {
            AggregateId = aggregateId;
            OccurredAtUtc = DateTime.UtcNow;
            Id = Guid.NewGuid();
        }

        protected PersistedDomainEventBase(TIdentifier aggregateId, Guid? correlationId, Guid? causationId) : this(aggregateId)
        {
            CorrelationId = correlationId;
            CausationId = causationId;
        }

        public DateTime OccurredAtUtc { get; }

        public Guid Id { get; }

        public Guid? CorrelationId { get; }

        public Guid? CausationId { get; }

        public TIdentifier AggregateId { get; }
    }

    //TODO: make a fluent builder thingy
}
