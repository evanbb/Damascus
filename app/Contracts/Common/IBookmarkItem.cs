using System;

namespace Damascus.Example.Contracts
{
    public interface IBookmarkItem
    {
        string Label { get; }
        Guid Id { get; }
    }
}
