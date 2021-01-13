using System;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class BookmarkCreated : IDomainEvent
    {
        public BookmarkCreated(Guid bookmarkId, Guid parentFolderId, Position position)
        {
            BookmarkId = bookmarkId;
            ParentFolderId = parentFolderId;
            Position = position;
        }

        public Guid BookmarkId { get; }
        public Guid ParentFolderId { get; }
        public Position Position { get; }
    }
}
