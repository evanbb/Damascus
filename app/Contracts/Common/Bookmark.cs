using System;
namespace Damascus.Example.Contracts
{
    public record Bookmark(Guid Id, string Label, string Url) : IBookmarkItem;
}
