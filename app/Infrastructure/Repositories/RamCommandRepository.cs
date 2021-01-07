using Damascus.Core;
using Damascus.Domain.Abstractions;
using Damascus.Example.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Damascus.Example.Infrastructure
{
    public class RamCommandRepository : IWidgetCommandRepository
    {
        private ConcurrentDictionary<Guid, Widget> _repo = new ConcurrentDictionary<Guid, Widget>();
        private readonly IReadOnlyCollection<IHandle<IDomainEvent>> _eventHandlers;

        public RamCommandRepository(IReadOnlyCollection<IHandle<IDomainEvent>> eventHandlers)
        {
            _eventHandlers = eventHandlers;
        }

        public async Task<Maybe<Widget>> FindAsync(Guid id)
        {
            var result = _repo.TryGetValue(id, out var widget)
                ? widget.ToMaybe<Widget>()
                : Maybe.Nothing;

            return result;
        }

        public async Task CommitAsync(Widget aggregate)
        {
            _repo[aggregate.Id] = aggregate;

            var events = aggregate.FlushEvents();

            events = events.Concat(new[]
            {
                new WidgetSnapshotEvent(aggregate)
            });

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
