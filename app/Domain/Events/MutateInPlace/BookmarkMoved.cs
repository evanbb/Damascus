using System;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class BookmarkMoved : IDomainEvent
    {
        public BookmarkMoved(Guid sourceFolderId, Guid destinationFolderId, Position position)
        {
            sourceFolderId.BetterNotBe(Guid.Empty, "Cannot move root folder");
            position.BetterNotBeNull(nameof(position));

            SourceFolderId = sourceFolderId;
            DestinationFolderId = destinationFolderId;
            Position = position;
        }

        public Guid SourceFolderId { get; }
        public Guid DestinationFolderId { get; }
        public Position Position { get; }
    }
}
