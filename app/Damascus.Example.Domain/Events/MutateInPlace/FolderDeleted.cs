using System;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class FolderDeleted : IDomainEvent
    {
        public FolderDeleted(Guid folderId)
        {
            folderId.BetterNotBe(Guid.Empty, "Folder ID");

            FolderId = folderId;
        }

        public Guid FolderId { get; }
    }
}
