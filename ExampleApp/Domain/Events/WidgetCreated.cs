using System;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class WidgetCreated : IDomainEvent
    {
        public WidgetCreated(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new InvalidOperationException("Widget ID was not provided");
            }

            WidgetId = id;
        }

        public Guid WidgetId { get; }
    }
}
