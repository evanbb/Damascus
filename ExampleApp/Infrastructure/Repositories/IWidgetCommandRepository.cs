using System;
using Damascus.Core;
using Damascus.Example.Domain;
using Damascus.Persistence.Abstractions.Cqrs;

namespace Damascus.Example.Infrastructure
{
    public interface IWidgetCommandRepository :
        ICommandRepository<Guid, Widget>
    {

    }
}
