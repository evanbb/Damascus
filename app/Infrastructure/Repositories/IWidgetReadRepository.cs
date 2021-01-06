using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Damascus.Domain.Abstractions;
using Damascus.Persistence.Abstractions.Cqrs;

namespace Damascus.Example.Infrastructure
{
    public interface IWidgetReadRepository :
        IQueryRepository<Guid, Contracts.Widget>, IHandle<IDomainEvent>
    {
        Task<IEnumerable<Contracts.Widget>> SearchAsync();
    }
}
