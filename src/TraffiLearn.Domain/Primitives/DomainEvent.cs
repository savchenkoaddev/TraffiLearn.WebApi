using MediatR;

namespace TraffiLearn.Domain.Primitives
{
    public abstract record DomainEvent : INotification
    {
        public Guid Id { get; } = Guid.NewGuid();

        protected DomainEvent() { }
    }
}
