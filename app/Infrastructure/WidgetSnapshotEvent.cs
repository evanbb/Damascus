using Damascus.Domain.Abstractions;
using Damascus.Example.Domain;

namespace Damascus.Example.Infrastructure
{
    public class WidgetSnapshotEvent : IDomainEvent
    {
        public WidgetSnapshotEvent(Widget widget)
        {
            Widget = widget.ToContract();
        }

        public Contracts.Widget Widget { get; }
    }
}
