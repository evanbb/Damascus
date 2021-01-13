using System;
using System.Collections.Generic;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public interface IFolderContent
    {
        Guid Id { get; }
        Maybe<T> Find<T>(Predicate<T> callback)
            where T : IFolderContent;
        IEnumerable<IDomainEvent> OnMoved(MutableFolder sourceFolder, MutableFolder destinationFolder, Position position);
        IEnumerable<IDomainEvent> OnAdded(MutableFolder destinationFolder, Position position);
        IEnumerable<IDomainEvent> OnDeleted();
    }
}
