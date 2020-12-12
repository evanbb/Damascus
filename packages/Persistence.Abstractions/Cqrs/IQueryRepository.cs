using System;
using Damascus.Domain.Abstractions;

namespace Damascus.Persistence.Abstractions.Cqrs
{
    public interface IQueryRepository<TIdentifier, TReadModel>
        where TIdentifier : IEquatable<TIdentifier>
        where TReadModel : class
    {
        Task<TReadModel> FindAsync(TIdentifier id);
    }
}
