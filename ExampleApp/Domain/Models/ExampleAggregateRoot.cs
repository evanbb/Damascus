using System;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class ExampleAggregateRoot : AggregateRootBase<Guid>
    {
        public ExampleAggregateRoot(Guid id) : base(id)
        {

        }
    }
}
