using System;
using System.Threading.Tasks;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Persistence.Abstractions.OptimisticLocking
{
    public interface IVersionedAggregateRepository<TIdentifier, TAggregate>
        where TIdentifier : IEquatable<TIdentifier>
        where TAggregate : class, IVersionedAggregateRoot<TIdentifier>
    {
        Task<Maybe<TAggregate>> FindAsync(TIdentifier id);
        Task CommitAsync(TAggregate aggregate);
    }
}
