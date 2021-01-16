using System;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class FolderMoved : IDomainEvent
    {
        public FolderMoved(Guid folderId, Guid destinationFolderId, Position position)
        {
            folderId.BetterNotBe(Guid.Empty, "Cannot move root folder");
            position.BetterNotBeNull(nameof(position));

            FolderId = folderId;
            DestinationFolderId = destinationFolderId;
            Position = position;
        }

        public Guid FolderId { get; }
        public Guid DestinationFolderId { get; }
        public Position Position { get; }
    }
}
