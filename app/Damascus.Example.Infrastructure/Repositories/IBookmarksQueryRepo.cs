using System;
using Damascus.Example.Contracts;
using Damascus.Persistence.Abstractions.Cqrs;

namespace Damascus.Example.Infrastructure
{
    public interface IBookmarksQueryRepo
        : IQueryRepository<Guid, BookmarksCollection>
    {
        void Handle(object @event);
    }
}
