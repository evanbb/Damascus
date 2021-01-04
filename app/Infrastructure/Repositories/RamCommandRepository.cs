using Damascus.Core;
using Damascus.Domain.Abstractions;
using Damascus.Example.Domain;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Damascus.Example.Infrastructure
{
    public class RamCommandRepository : IWidgetCommandRepository
    {
        private IDictionary<Guid, Widget> _repo = new Dictionary<Guid, Widget>();
        private readonly IReadOnlyCollection<IHandle<IDomainEvent>> _eventHandlers;

        public RamCommandRepository(IReadOnlyCollection<IHandle<IDomainEvent>> eventHandlers)
        {
            _eventHandlers = eventHandlers;
        }

        public async Task<Maybe<Widget>> FindAsync(Guid id)
        {
            var result = _repo.TryGetValue(id, out var widget)
                ? widget.ToMaybe<Widget>()
                : Maybe<Widget>.Nothing;

            return result;
        }

        public async Task CommitAsync(Widget aggregate)
        {
            _repo[aggregate.Id] = aggregate;

            var events = aggregate.FlushEvents();

            foreach (var handler in _eventHandlers)
            {
                foreach (var @event in events)
                {
                    handler.Handle(@event);
                }
            }
        }
    }
}
