using System;
using System.Collections.Generic;

namespace Damascus.Example.Contracts
{
    public record BookmarksCollection(Guid Id, IEnumerable<IBookmarkItem> Items);
}
