using TraffiLearn.SharedKernel.Primitives;

namespace TraffiLearn.Domain.Users.DomainEvents
{
    public sealed record UserCreatedDomainEvent(
        Guid UserId,
        string Email) : DomainEvent;
}
