using Microsoft.Extensions.Options;
using Stripe;
using System.Security;
using TraffiLearn.Application.Abstractions.Payments;
using TraffiLearn.Infrastructure.Services.Payments.Options;

namespace TraffiLearn.Infrastructure.Services.Payments
{
    internal sealed class StripeWebhookService : IStripeWebhookService
    {
        private readonly StripeSettings _stripeSettings;

        public StripeWebhookService(
            IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
        }

        public Event ValidateStripeEvent(string json, string signatureHeader)
        {
            try
            {
                return EventUtility.ConstructEvent(
                    json,
                    signatureHeader,
                    _stripeSettings.WebhookSecret);
            }
            catch (StripeException ex)
            {
                throw new SecurityException("Invalid Stripe webhook signature.", ex);
            }
        }
    }
}
