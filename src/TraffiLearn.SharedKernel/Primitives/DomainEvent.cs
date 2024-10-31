using MediatR;

namespace TraffiLearn.SharedKernel.Primitives
{
    public abstract record DomainEvent : INotification
    {
        public Guid Id { get; } = Guid.NewGuid();

        protected DomainEvent() { }
    }
}
