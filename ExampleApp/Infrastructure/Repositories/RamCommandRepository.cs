using Damascus.Core;
using Damascus.Example.Domain;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Damascus.Example.Infrastructure
{
    public class RamCommandRepository : IWidgetCommandRepository
    {
        private IDictionary<Guid, Widget> _repo = new Dictionary<Guid, Widget>();

        public Task<Maybe<Widget>> FindAsync(Guid id)
        {
            var result = _repo.TryGetValue(id, out var widget)
                ? widget.ToMaybe<Widget>()
                : Maybe<Widget>.Nothing;

            return Task.FromResult(result);
        }

        public Task CommitAsync(Widget aggregate)
        {
            _repo[aggregate.Id] = aggregate;

            return Task.CompletedTask;
        }
    }
}
