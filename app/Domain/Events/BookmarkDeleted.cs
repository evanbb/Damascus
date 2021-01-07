using System;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class BookmarkDeleted : IDomainEvent
    {
        public BookmarkDeleted(Guid id)
        {
            id.BetterNotBe(Guid.Empty, "ID cannot be empty");

            Id = id;
        }

        public Guid Id { get; }
    }
}
