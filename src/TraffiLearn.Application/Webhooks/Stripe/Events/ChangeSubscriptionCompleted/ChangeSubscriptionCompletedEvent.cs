using MediatR;

namespace TraffiLearn.Application.Webhooks.Stripe.Events.ChangeSubscriptionCompleted
{
    internal sealed record ChangeSubscriptionCompletedEvent(
        Guid SubscriptionPlanId,
        Guid UserId,
        string? Metadata) : INotification;
}
