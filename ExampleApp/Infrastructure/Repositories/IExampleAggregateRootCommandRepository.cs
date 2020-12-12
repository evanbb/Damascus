using Damascus.Example.Domain;
using Damascus.Persistence.Abstractions.Cqrs;

namespace Damascus.Example.Infrastructure
{
    public interface IExampleAggregateRootComandRepository :
        ICommandRepository<Guid, ExampleAggregateRoot>
    {

    }
}
