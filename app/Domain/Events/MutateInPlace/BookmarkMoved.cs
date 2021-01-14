using System;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class BookmarkMoved : IDomainEvent
    {
        public BookmarkMoved(Guid bookmarkId, Guid destinationFolderId, Position position)
        {
            bookmarkId.BetterNotBe(Guid.Empty, "Bookmarks should have a non-empty identifer");
            position.BetterNotBeNull(nameof(position));

            BookmarkId = bookmarkId;
            DestinationFolderId = destinationFolderId;
            Position = position;
        }

        public Guid BookmarkId { get; }
        public Guid DestinationFolderId { get; }
        public Position Position { get; }
    }
}
