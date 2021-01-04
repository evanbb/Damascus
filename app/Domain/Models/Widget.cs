using System;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class Widget : AggregateRootBase<Guid>
    {
        private Widget(Guid id, string description, Motor motor) : base(id)
        {
            Description = description;
            Motor = motor;
        }

        public static Widget Rehydrate(Guid id, string description, Motor motor)
        {
            return new Widget(id, description, motor);
        }

        public static Widget CreateNew(string description, Motor motor)
        {
            var widgetId = Guid.NewGuid();
            var widget = new Widget(widgetId, description, motor);
            widget.Emit(new WidgetCreated(widgetId));

            return widget;
        }

        public string Description { get; private set; }

        public Motor Motor { get; private set; }

        public void UpdateDescription(string newDescription)
        {
            if (!newDescription.HasContent())
            {
                throw new InvalidOperationException("New description must be provided");
            }

            Description = newDescription;

            Emit(new DescriptionUpdated(Id, Description));
        }
    }
}
