using TraffiLearn.SharedKernel.Primitives;

namespace TraffiLearn.Domain.Users.DomainEvents
{
    public sealed record SubscriptionChangedDomainEvent(
        Guid UserId,
        DateTime PlanExpiresOn) : DomainEvent;
}
