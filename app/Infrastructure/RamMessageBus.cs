using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Infrastructure
{
    public class RamMessageBus : IMessageBus
    {
        private readonly Dictionary<string, IEnumerable<Action<IDomainEvent>>> _subscriptions =
            new Dictionary<string, IEnumerable<Action<IDomainEvent>>>();

        public RamMessageBus(Dictionary<string, IEnumerable<Action<IDomainEvent>>> subscriptions)
        {
            _subscriptions = subscriptions;
        }

        public void Publish(string topic, IDomainEvent message)
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
