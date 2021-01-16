using System;
using System.Collections.Generic;

namespace Damascus.Example.Contracts
{
    public record Folder(Guid Id, string Label, IEnumerable<Guid> Items) : IBookmarkItem;
}
