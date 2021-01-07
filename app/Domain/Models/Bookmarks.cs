using Damascus.Core;
using Damascus.Domain.Abstractions;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Damascus.Example.Domain
{
    public class Bookmarks : AggregateRootBase<Guid>
    {
        private Bookmarks(Guid id, Dictionary<string, IEnumerable<Bookmark>> bookmarks) : base(id)
        {
            bookmarks.BetterNotBeNull("Bookmarks");
            bookmarks.BetterHaveNoNulls("Bookmarks");

            Collection = new Dictionary<string, IEnumerable<Bookmark>>(bookmarks);
        }

        public Dictionary<string, IEnumerable<Bookmark>> Collection { get; private set; }

        public Bookmarks CreateNew(Dictionary<string, IEnumerable<Bookmark>> bookmarks)
        {
            return new Bookmarks(Guid.NewGuid(), bookmarks);
        }

        public Bookmarks Rehydrate(Guid id, Dictionary<string, IEnumerable<Bookmark>> bookmarks)
        {
            return new Bookmarks(id, bookmarks);
        }

        public void AddBookmark(Bookmark bookmark, string path, int position)
        {
            bookmark.BetterNotBeNull();
            path.BetterNotBeNull();
            position.BetterBe(p => p > 0, "Position must be positive");

            var exists = Collection.TryGetValue(path, out var list);

            if (!exists)
            {
                list = new List<Bookmark>();
            }

            list = list.ToList();

            ((List<Bookmark>)list).Insert(position - 1, bookmark);

            Collection[path] = list;
        }

        public void RemoveBookmark(string path, Guid id)
        {
            path.BetterNotBeNull();
            id.BetterNotBe(Guid.Empty, "Must specify a bookmark to remove.");

            var exists = Collection.TryGetValue(path, out var list);

            if (!exists)
            {
                return;
            }

            Collection[path] = list.Where(i => !i.Id.Equals(id));
        }

        public void MoveBookmark(Guid id, string from, string to, int position)
        {
            from.BetterNotBeNull("From");
            to.BetterNotBeNull("To");
            id.BetterNotBe(Guid.Empty, "Must specify a bookmark to remove.");
            position.BetterBe(p => p > 0, "Position must be greater than zero");

            var sourceExists = Collection.TryGetValue(from, out var source);

            if (!sourceExists)
            {
                throw new InvalidOperationException("Unable to move bookmark from non-existent source");
            }

            var bookmarkToMove = source.SingleOrDefault(e => e.Id == Id);
            bookmarkToMove.BetterNotBeNull("Bookmark to move");

            var destinationExists = Collection.TryGetValue(to, out var destination);

            if (!destinationExists)
            {
                destination = new Bookmark[] { };
            }

            var newList = destination.ToList();

            position.BetterBe(p => p < newList.Count, "Cannot move bookmark out of bounds at destination");

            newList.Insert(position, bookmarkToMove);

            Collection[to] = newList;
        }

        public void AddFolder(string path)
        {
            path.BetterNotBeNull("Path");
            path.BetterNotBe(string.Empty, "Cannot create a new root folder");

            var alreadyExists = Collection.ContainsKey(path);

            if (alreadyExists)
            {
                throw new InvalidOperationException("Folder already exists");
            }

            Collection[path] = new Bookmark[] { };
        }

        public void RemoveFolder(string path)
        {
            path.BetterNotBeNull("Path");
            path.BetterNotBe(string.Empty, "Cannot delete root folder");

            var alreadyExists = Collection.ContainsKey(path);

            if (!alreadyExists)
            {
                return;
            }

            Collection.Remove(path);
        }

        public void MoveFolder(string from, string to)
        {
            from.BetterNotBeNull("From");
            to.BetterNotBeNull("To");


        }
    }
}
