namespace Damascus.Example.Infrastructure
{
    public interface IMessageBus
    {
        void Publish(string topic, object message);
    }
}
