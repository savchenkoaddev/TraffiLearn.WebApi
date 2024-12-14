using Stripe;

namespace TraffiLearn.Application.Abstractions.Payments
{
    public interface IStripeWebhookService
    {
        Event ValidateStripeEvent(string json, string signatureHeader);
    }
}
