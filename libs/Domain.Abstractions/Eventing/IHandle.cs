namespace Damascus.Domain.Abstractions
{
    public interface IHandle<TDomainEvent>
        where TDomainEvent : class, IDomainEvent
    {
        public interface And<TOtherDomainEvent> : IHandle<TOtherDomainEvent>
            where TOtherDomainEvent : class, IDomainEvent
        {

        }

        void Handle(TDomainEvent theEvent);
    }
}
