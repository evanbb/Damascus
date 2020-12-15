using System;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class Widget : AggregateRootBase<Guid>
    {
        private Widget(Guid id, string description, Motor motor) : base(id)
        {

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
    }
}
