namespace TraffiLearn.Application.Webhooks.Stripe
{
    public enum StripeEventType
    {
        CheckoutSessionCompleted,
        InvoicePaymentSucceeded,
        PaymentIntentSucceeded,
        PaymentIntentFailed,
        Unknown
    }
}
