using System;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class BookmarkReaddressed : IDomainEvent
    {
        public BookmarkReaddressed(Guid bookmarkId, Uri uri)
        {
            bookmarkId.BetterNotBe(Guid.Empty, "Bookmark ID");
            uri.BetterNotBeNull("Bookmark address");

            BookmarkId = bookmarkId;
            Uri = uri;
        }

        public Guid BookmarkId { get; }
        public Uri Uri { get; }
    }
}
