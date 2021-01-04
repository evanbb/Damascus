using System;
using System.Threading.Tasks;
using Damascus.Core;

namespace Damascus.Persistence.Abstractions.Cqrs
{
    public interface IQueryRepository<TIdentifier, TReadModel>
        where TIdentifier : IEquatable<TIdentifier>
        where TReadModel : class
    {
        Task<Maybe<TReadModel>> FindAsync(TIdentifier id);
    }
}
