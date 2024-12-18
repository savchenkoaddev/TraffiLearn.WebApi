using TraffiLearn.SharedKernel.Primitives;

namespace TraffiLearn.Domain.Users.DomainEvents
{
    public sealed record SubscriptionRenewedDomainEvent(
        Guid UserId,
        DateTime PlanExpiresOn) : DomainEvent;
}