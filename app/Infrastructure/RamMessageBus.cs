using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Damascus.Core;

namespace Damascus.Example.Infrastructure
{
    public class RamMessageBus : IMessageBus
    {
        private readonly ConcurrentDictionary<string, List<Action<object>>> _callbacks =
            new ConcurrentDictionary<string, List<Action<object>>>();

        public void Publish(string topic, object message)
        {
            if (message.IsNull())
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (!_callbacks.TryGetValue(topic, out var callbacks))
            {
                return;
            }

            callbacks.ForEach(a => a(message));
        }

        public void Subscribe(string topic, Action<object> callback)
        {
            _callbacks.AddOrUpdate(topic, (_) => new List<Action<object>>(), (_, list) =>
            {
                list.Add(callback);
                return list;
            });
        }
    }
}
