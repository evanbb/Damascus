using System;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class FolderRenamed: IDomainEvent
    {
        public FolderRenamed(Guid folderId, string newName)
        {
            folderId.BetterNotBe(Guid.Empty, "Folder ID");
            newName.BetterNotBeNull("Folder Name");

            FolderId = folderId;
            NewName = newName;
        }

        public Guid FolderId { get; }
        public string NewName { get; }
    }
}
