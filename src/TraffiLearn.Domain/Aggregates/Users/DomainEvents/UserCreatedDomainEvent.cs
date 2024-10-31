using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Aggregates.Users.DomainEvents
{
    public sealed record UserCreatedDomainEvent(
        Guid UserId,
        string Email) : DomainEvent;
}
