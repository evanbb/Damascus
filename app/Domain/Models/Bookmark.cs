using Damascus.Core;
using Damascus.Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Damascus.Example.Domain
{
    public class Bookmark : Entity<Guid>, IFolderContent
    {
        public Bookmark(
            Guid id,
            string url,
            string name,
            DateTimeOffset createdOn,
            DateTimeOffset lastModifiedOn
        ) : base(id)
        {
            id.BetterNotBe(i => i == Guid.Empty, "Unable to create bookmark with empty identifier");
            url.BetterNotBeNull("Url");
            name.BetterNotBeNull("Name");

            Url = url;
            Name = name;
            CreatedOn = createdOn;
            LastModifiedOn = lastModifiedOn;
        }

        public static Bookmark CreateNew(string url, string name)
        {
            return new Bookmark(Guid.NewGuid(), url, name, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);
        }

        public static Bookmark Rehydrate(
            Guid id,
            string url,
            string name,
            DateTimeOffset createdOn,
            DateTimeOffset lastModifiedOn)
        {
            return new Bookmark(id, url, name, createdOn, lastModifiedOn);
        }

        public string Url { get; private set; }
        public string Name { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset LastModifiedOn { get; private set; }

        public Maybe<IFolderContent> Find(Predicate<IFolderContent> callback)
        {
            if (callback(this))
            {
                return this;
            }

            return Maybe.Nothing;
        }

        public IEnumerable<IDomainEvent> Delete()
        {
            yield return new BookmarkDeleted(this.Id);
        }
    }
}
