using System;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class BookmarkRenamed : IDomainEvent
    {
        public BookmarkRenamed(Guid bookmarkId, string newName)
        {
            bookmarkId.BetterNotBe(Guid.Empty, "Bookmark ID");
            newName.BetterNotBeNull("Bookmark name");

            BookmarkId = bookmarkId;
            NewName = newName;
        }

        public Guid BookmarkId { get; }
        public string NewName { get; }
    }
}
