using System;
using System.Collections.Generic;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public interface IFolderContent
    {
        Maybe<IFolderContent> Find(Predicate<IFolderContent> callback);
        IEnumerable<IDomainEvent> Delete();
    }
}
