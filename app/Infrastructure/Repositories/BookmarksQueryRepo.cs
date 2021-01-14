using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Damascus.Core;
using Damascus.Example.Contracts;

namespace Damascus.Example.Infrastructure
{
    public class BookmarksCollectionQueryRepo : IBookmarksQueryRepo
    {
        private readonly Dictionary<Guid, BookmarksCollection> _repo =
            new Dictionary<Guid, BookmarksCollection>();

        public async Task<Maybe<BookmarksCollection>> FindAsync(Guid id)
        {
            if (!_repo.ContainsKey(id))
            {
                return Maybe.Nothing;
            }

            return _repo[id];
        }

        public void Handle(object e)
        {
            var snapshot = e as BookmarksCollectionSnapshotUpdated;

            if (snapshot.IsNull())
            {
                return;
            }

            _repo[snapshot.Collection.Id] = snapshot.Collection;
        }
    }
}
