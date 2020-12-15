using System;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class SnapshottingWidget : AggregateRootBase<Guid>
    {
        private SnapshottingWidget(Guid id, Motor motor) : base(id)
        {

        }

        public SnapshottingWidget Create(Guid id, Motor motor)
        {
            return new SnapshottingWidget(id, motor);
        }

        public SnapshottingWidget Create(Motor motor)
        {
            var widgetId = Guid.NewGuid();
            var widget = new SnapshottingWidget(widgetId, motor);
            widget.Emit(new WidgetCreated(widgetId));

            return widget;
        }
    }
}
