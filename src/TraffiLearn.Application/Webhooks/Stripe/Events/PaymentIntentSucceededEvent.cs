using MediatR;

namespace TraffiLearn.Application.Webhooks.Stripe.Events
{
    internal sealed record PaymentIntentSucceededEvent(
        Guid SubscriptionPlanId,
        Guid UserId) : INotification;
}
