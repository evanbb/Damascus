using System;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class WidgetCreated : IDomainEvent
    {
        public WidgetCreated(Guid id)
        {
            WidgetId = id;
        }

        public Guid WidgetId { get; }
    }
}
