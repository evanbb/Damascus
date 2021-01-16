using System;
using System.Linq;
using System.Collections.Generic;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class MutableFolder : Entity<Guid>, IFolderContent
    {
        private MutableFolder(IEnumerable<IFolderContent> contents)
            : this(Guid.Empty, "ROOT", contents) { }

        private MutableFolder(Guid id, string name, IEnumerable<IFolderContent> contents) : base(id)
        {
            contents.BetterNotBeNull("Folder contents");
            contents.BetterNotHaveNulls("Folder contents");
            name.BetterNotBeNull("Folder name");

            Contents = contents;
            Name = name;
        }

        public static MutableFolder CreateRoot(IEnumerable<IFolderContent> contents)
        {
            return new MutableFolder(contents);
        }

        public static MutableFolder CreateNew(string name)
        {
            return new MutableFolder(Guid.NewGuid(), name, Enumerable.Empty<IFolderContent>());
        }

        public static MutableFolder Rehydrate(Guid id, string name, IEnumerable<IFolderContent> contents)
        {
            return new MutableFolder(id, name, contents);
        }

        public IEnumerable<IFolderContent> Contents { get; private set; }
        public string Name { get; private set; }

        public IEnumerable<IDomainEvent> Add(IFolderContent item, Position position)
        {
            item.BetterNotBeNull(nameof(item));
            position.BetterNotBeNull(nameof(position));

            var list = Contents.ToList();

            list.Insert(position.NonZeroIndex - 1, item);

            Contents = list;

            foreach (var e in item.OnAdded(this, position))
            {
                yield return e;
            }
        }

        public IEnumerable<IDomainEvent> DeleteItem(Guid childId)
        {
            var child = Contents
                .SingleOrDefault(c => c.Id == childId);

            if (child.IsNull())
            {
                yield break;
            }

            Contents = Contents.Where(c => c.Id != childId).ToList();

            foreach (var e in child.OnDeleted())
            {
                yield return e;
            }
        }

        public Maybe<T> Find<T>(Predicate<T> callback)
            where T : IFolderContent
        {
            if (this is T)
            {
                var t = (T)(object)this;
                if (callback(t))
                {
                    return t;
                }
            }

            foreach (var item in Contents)
            {
                var result = item.Find(callback);
                if (result.HasValue)
                {
                    return result;
                }
            }

            return Maybe.Nothing;
        }

        public IEnumerable<IDomainEvent> MoveItem(Guid itemId, MutableFolder destination, Position position)
        {
            destination.BetterNotBeNull(nameof(destination));
            position.BetterNotBeNull(nameof(position));

            var item = Contents.SingleOrDefault(c => c.Id == itemId);

            if (item.IsNull())
            {
                throw new InvalidOperationException($"Unable to find element {itemId} in folder {Id}");
            }

            destination.Insert(item, position);

            Contents = Contents.Where(c => c.Id != itemId).ToList();

            foreach (var e in item.OnMoved(destination, position))
            {
                yield return e;
            }
        }

        public IEnumerable<IDomainEvent> Rename(string newName)
        {
            newName.BetterNotBeNull("Folder name");

            Name = newName;

            yield return new FolderRenamed(Id, newName);
        }

        public IEnumerable<IDomainEvent> OnDeleted()
        {
            foreach (var item in Contents)
            {
                foreach (var e in item.OnDeleted())
                {
                    yield return e;
                }
            }

            yield return new FolderDeleted(Id);
        }

        public IEnumerable<IDomainEvent> OnMoved(MutableFolder destinationFolder, Position position)
        {
            destinationFolder.BetterNotBeNull(nameof(destinationFolder));
            position.BetterNotBeNull(nameof(position));

            destinationFolder.Id.BetterNotBe(Id, "Cannot insert folder into itself");

            yield return new FolderMoved(Id, destinationFolder.Id, position);
        }

        public IEnumerable<IDomainEvent> OnAdded(MutableFolder destinationFolder, Position position)
        {
            destinationFolder.BetterNotBeNull(nameof(destinationFolder));
            position.BetterNotBeNull(nameof(position));

            destinationFolder.Id.BetterNotBe(Id, "Cannot insert folder into itself");

            yield return new FolderCreated(Id, destinationFolder.Id, position);
        }

        public Maybe<MutableFolder> ParentOf(Guid childId)
        {
            foreach (var child in Contents)
            {
                if (child.Id == childId)
                {
                    return this;
                }

                if (child is MutableBookmark)
                {
                    continue;
                }

                var result = ((MutableFolder)child).ParentOf(childId);

                if (result.HasValue)
                {
                    return result;
                }
            }

            return Maybe.Nothing;
        }

        private void Insert(IFolderContent item, Position position)
        {
            item.BetterNotBeNull("Item");
            position.BetterNotBeNull("Position");

            var list = Contents.ToList();

            list.Insert(position.NonZeroIndex - 1, item);

            Contents = list;
        }
    }
}
