using System;
using System.Collections.Generic;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Infrastructure
{
    public class RamMessageBus : IMessageBus
    {
        public delegate void MessageHandler(object @event);
        
        private readonly Dictionary<string, IEnumerable<MessageHandler>> _subscriptions =
            new Dictionary<string, IEnumerable<MessageHandler>>();

        public RamMessageBus(Dictionary<string, IEnumerable<MessageHandler>> subscriptions)
        {
            _subscriptions = subscriptions;
        }

        public void Publish(string topic, object message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (!_subscriptions.TryGetValue(topic, out var callbacks))
            {
                return;
            }

            foreach(var callback in callbacks)
            {
                callback(message);
            }
        }
    }
}
