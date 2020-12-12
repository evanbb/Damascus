using System;
using System.Collections.Generic;

namespace Damascus.Domain.Abstractions
{
    public abstract class AggregateRootBase<TIdentifier> : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        private List<IDomainEvent> _events = new List<IDomainEvent>();

        protected AggregateRootBase(TIdentifier id)
        {
            Id = id;
        }

        public IEnumerable<IDomainEvent> FlushEvents()
        {
            var result = _events;

            _events = new List<IDomainEvent>();

            return result;
        }

        public TIdentifier Id { get; }

        protected void Emit(IDomainEvent theEvent)
        {
            _events.Add(theEvent);
        }
    }
}
