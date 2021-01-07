using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Damascus.Core;
using Damascus.Domain.Abstractions;
using Damascus.Example.Contracts;

namespace Damascus.Example.Infrastructure
{
    public class RamReadRepository : IWidgetReadRepository
    {
        private readonly ConcurrentDictionary<Guid, Widget> _widgets = new ConcurrentDictionary<Guid, Widget>();

        public async Task<Maybe<Widget>> FindAsync(Guid id)
        {
            if (!_widgets.TryGetValue(id, out var result))
            {
                return Maybe.Nothing;
            }

            return result;
        }

        public async Task<IEnumerable<Widget>> SearchAsync()
        {
            return _widgets.Select(kvp => kvp.Value);
        }

        public void Handle(IDomainEvent theEvent)
        {
            var snapshotEvent = theEvent as WidgetSnapshotEvent;

            if (snapshotEvent is null)
            {
                return;
            }

            _widgets.AddOrUpdate(snapshotEvent.Widget.Id, snapshotEvent.Widget, (_, __) => snapshotEvent.Widget);
        }
    }
}
