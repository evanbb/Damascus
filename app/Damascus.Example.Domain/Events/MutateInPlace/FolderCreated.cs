using System;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class FolderCreated : IDomainEvent
    {
        public FolderCreated(Guid folderId, Guid parentFolderId, Position position)
        {
            position.BetterNotBeNull(nameof(position));
            folderId.BetterNotBe(Guid.Empty, "Cannot recreate root folder");

            FolderId = folderId;
            ParentFolderId = parentFolderId;
            Position = position;
        }

        public Guid FolderId { get; }
        public Guid ParentFolderId { get; }
        public Position Position { get; }
    }
}
