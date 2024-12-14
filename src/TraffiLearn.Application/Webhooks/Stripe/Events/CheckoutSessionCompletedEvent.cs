using MediatR;

namespace TraffiLearn.Application.Webhooks.Stripe.Events
{
    internal sealed record CheckoutSessionCompletedEvent(
        Guid SubscriptionPlanId,
        Guid UserId) : INotification;
}
