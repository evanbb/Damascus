using System;
using System.Collections.Generic;
using Damascus.Domain.Abstractions.Events;

namespace Damascus.Domain.Abstractions
{
    public interface IAggregateRoot<TIdentifier>
      where TIdentifier : IEquatable<TIdentifier>
    {
        TIdentifier Id { get; }
        IEnumerable<IDomainEvent> FlushEvents();
    }
}
