using Damascus.Core;
using Damascus.Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Damascus.Example.Domain
{
    public class MutableBookmark : Entity<Guid>, IFolderContent
    {
        public MutableBookmark(
            Guid id,
            Uri uri,
            string name,
            DateTimeOffset createdOn,
            DateTimeOffset lastModifiedOn
        ) : base(id)
        {
            id.BetterNotBe(i => i == Guid.Empty, "Unable to create bookmark with empty identifier");
            uri.BetterNotBeNull("URI");
            name.BetterNotBeNull("Name");
            Uri = uri;
            Name = name;
            CreatedOn = createdOn;
            LastModifiedOn = lastModifiedOn;
        }

        public static MutableBookmark CreateNew(Uri uri, string name)
        {
            return new MutableBookmark(
                Guid.NewGuid(),
                uri,
                name,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow
            );
        }

        public static MutableBookmark Rehydrate(
            Guid id,
            Uri uri,
            string name,
            DateTimeOffset createdOn,
            DateTimeOffset lastModifiedOn)
        {
            return new MutableBookmark(id, uri, name, createdOn, lastModifiedOn);
        }

        public Uri Uri { get; private set; }
        public string Name { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset LastModifiedOn { get; private set; }

        public IEnumerable<IDomainEvent> Delete()
        {
            yield return new BookmarkDeleted(Id);
        }

        public Maybe<T> Find<T>(Predicate<T> callback) where T : IFolderContent
        {
            if (this is T)
            {
                var t = (T)(object)this;
                if (callback(t))
                {
                    return t;
                }
            }

            return Maybe.Nothing;
        }

        public IEnumerable<IDomainEvent> Rename(string newName)
        {
            newName.BetterNotBeNull(nameof(newName));

            Name = newName;

            yield return new BookmarkRenamed(Id, newName);
        }

        public IEnumerable<IDomainEvent> Readdress(Uri uri)
        {
            uri.BetterNotBeNull(nameof(uri));

            Uri = uri;

            yield return new BookmarkReaddressed(Id, uri);
        }

        public IEnumerable<IDomainEvent> OnMoved(MutableFolder destinationFolder, Position position)
        {
            destinationFolder.BetterNotBeNull(nameof(destinationFolder));
            position.BetterNotBeNull(nameof(position));

            yield return new BookmarkMoved(Id, destinationFolder.Id, position);
        }

        public IEnumerable<IDomainEvent> OnAdded(MutableFolder destinationFolder, Position position)
        {
            destinationFolder.BetterNotBeNull(nameof(destinationFolder));
            position.BetterNotBeNull(nameof(position));

            yield return new BookmarkCreated(Id, destinationFolder.Id, position);
        }

        public IEnumerable<IDomainEvent> OnDeleted()
        {
            yield return new BookmarkDeleted(Id);
        }
    }
}
