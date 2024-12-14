using MediatR;

namespace TraffiLearn.Application.Webhooks.Stripe.Events
{
    public sealed class PaymentIntentSucceededEvent : INotification
    {
    }
}
