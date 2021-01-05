using System;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Infrastructure
{
    public interface IMessageBus
    {
        void Publish(string topic, IDomainEvent message);
    }
}
