using System;
using System.Collections.Generic;

namespace Damascus.Domain.Abstractions
{
    public abstract class AggregateRootBase<TIdentifier> : Entity<TIdentifier>, IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        private List<IDomainEvent> _events = new List<IDomainEvent>();

        protected AggregateRootBase(TIdentifier id) : base(id) { }

        public IEnumerable<IDomainEvent> FlushEvents()
        {
            var result = _events;

            _events = new List<IDomainEvent>();

            return result;
        }

        protected void Emit(IDomainEvent theEvent)
        {
            _events.Add(theEvent);
        }

        protected void Emit(IEnumerable<IDomainEvent> theEvents)
        {
            foreach(var e in theEvents)
            {
                Emit(e);
            }
        }
    }
}
