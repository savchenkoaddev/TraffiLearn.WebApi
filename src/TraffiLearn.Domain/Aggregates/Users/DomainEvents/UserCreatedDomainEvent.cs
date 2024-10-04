using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Aggregates.Users.DomainEvents
{
    public sealed record UserCreatedDomainEvent(
        UserId UserId,
        Email Email) : DomainEvent;
}
