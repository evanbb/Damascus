using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Damascus.Core;
using Damascus.Example.Contracts;
using Damascus.Example.Domain;

namespace Damascus.Example.Infrastructure
{
    public class MutableBookmarksCommandRepo : IMutableBookmarksCommandRepo
    {
        private readonly Dictionary<Guid, MutableBookmarksCollection> _collection =
            new Dictionary<Guid, MutableBookmarksCollection>();

        private readonly IMessageBus _bus;
        private static readonly string _eventChannel = typeof(Contracts.BookmarksCollection).FullName;

        public MutableBookmarksCommandRepo(IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task CommitAsync(MutableBookmarksCollection aggregate)
        {
            var domainEvents = aggregate.FlushEvents().ToList();
            var events = domainEvents.Cast<object>().Concat(new[] { new BookmarksCollectionSnapshotUpdated(aggregate.ToContract()) });

            foreach (var e in events)
            {
                _bus.Publish(_eventChannel, e);
            }

            _collection[aggregate.Id] = aggregate;
        }

        public async Task<Maybe<MutableBookmarksCollection>> FindAsync(Guid id)
        {
            if (_collection.ContainsKey(id))
            {
                return _collection[id];
            }

            return Maybe.Nothing;
        }
    }
}
