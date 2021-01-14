using System;
using Damascus.Example.Domain;
using Damascus.Persistence.Abstractions.Cqrs;

namespace Damascus.Example.Infrastructure
{
    public interface IMutableBookmarksCommandRepo
        : ICommandRepository<Guid, MutableBookmarksCollection> { }
}
