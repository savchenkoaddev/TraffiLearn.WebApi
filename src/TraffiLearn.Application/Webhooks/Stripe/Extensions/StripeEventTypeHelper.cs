using Stripe;

namespace TraffiLearn.Application.Webhooks.Stripe.Extensions
{
    public static class StripeEventTypeHelper
    {
        public static StripeEventType GetStripeEventType(this Event stripeEvent)
        {
            return stripeEvent.Type switch
            {
                "checkout.session.completed" => StripeEventType.CheckoutSessionCompleted,
                "invoice.payment_succeeded" => StripeEventType.InvoicePaymentSucceeded,
                "payment_intent.succeeded" => StripeEventType.PaymentIntentSucceeded,
                "payment_intent.failed" => StripeEventType.PaymentIntentFailed,
                _ => StripeEventType.Unknown,
            };
        }
    }
}
