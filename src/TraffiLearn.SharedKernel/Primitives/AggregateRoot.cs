namespace TraffiLearn.SharedKernel.Primitives
{
    public abstract class AggregateRoot<TId> : Entity<TId>, IHasDomainEvents
        where TId : notnull
    {
        private readonly List<DomainEvent> _domainEvents = new();

        protected AggregateRoot(TId id)
            : base(id)
        { }

        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected void RaiseDomainEvent(DomainEvent domainEvent) =>
            _domainEvents.Add(domainEvent);
    }
}
