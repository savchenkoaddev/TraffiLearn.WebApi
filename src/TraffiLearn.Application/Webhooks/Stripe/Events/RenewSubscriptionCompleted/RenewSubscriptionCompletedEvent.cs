using MediatR;

namespace TraffiLearn.Application.Webhooks.Stripe.Events.RenewSubscriptionCompleted
{
    public sealed record RenewSubscriptionCompletedEvent(
        Guid UserId,
        string? Metadata) : INotification;
}
