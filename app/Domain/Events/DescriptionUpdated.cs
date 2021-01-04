using System;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class DescriptionUpdated : IDomainEvent
    {
        public DescriptionUpdated(Guid widgetId, string description)
        {
            WidgetId = widgetId;
            Description = description;
        }

        public Guid WidgetId { get; }
        public string Description { get; }
    }
}
