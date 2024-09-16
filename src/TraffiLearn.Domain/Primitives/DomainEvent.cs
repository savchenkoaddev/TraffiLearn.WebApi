using MediatR;

namespace TraffiLearn.Domain.Primitives
{
    public record DomainEvent(Guid Id) : INotification;
}
