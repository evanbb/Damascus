using System;
namespace Damascus.Example.Infrastructure
{
    public interface IMessageBus
    {
        void Publish(string topic, object message);
        void Subscribe(string topic, Action<object> callback);
    }
}
