using System;

namespace Damascus.Domain.Abstractions.Events
{
    public interface IPersistedDomainEvent<TIdentifier> : IDomainEvent
        where TIdentifier : IEquatable<TIdentifier>
    {
        Guid Id { get; }
        TIdentifier AggregateId { get; }
        DateTime OccurredAtUtc { get; }
        Guid? CorrelationId { get; }
        Guid? CausationId { get; }
    }
}
