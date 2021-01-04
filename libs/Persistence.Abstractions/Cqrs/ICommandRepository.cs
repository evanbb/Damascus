using System;
using System.Threading.Tasks;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Persistence.Abstractions.Cqrs
{
    public interface ICommandRepository<TIdentifier, TAggregateRoot>
        where TIdentifier : IEquatable<TIdentifier>
        where TAggregateRoot : class, IAggregateRoot<TIdentifier>
    {
        Task<Maybe<TAggregateRoot>> FindAsync(TIdentifier id);
        Task CommitAsync(TAggregateRoot aggregate);
    }
}
