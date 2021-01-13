using System;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class MutableBookmarksCollectionCreated : IDomainEvent
    {
        public MutableBookmarksCollectionCreated(Guid id)
        {
            id.BetterNotBe(Guid.Empty, "Cannot create bookmarks collection with empty ID");
            Id = id;
        }

        public Guid Id { get; }
    }
}
