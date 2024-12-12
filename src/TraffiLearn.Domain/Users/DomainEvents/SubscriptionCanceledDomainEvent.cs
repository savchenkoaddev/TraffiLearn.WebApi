using TraffiLearn.SharedKernel.Primitives;

namespace TraffiLearn.Domain.Users.DomainEvents
{
    public sealed record SubscriptionCanceledDomainEvent(
        Guid UserId,
        Guid SubscriptionId,
        DateTime CanceledAt,
        string? Reason) : DomainEvent;
}
