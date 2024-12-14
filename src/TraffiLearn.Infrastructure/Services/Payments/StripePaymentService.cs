using Microsoft.Extensions.Options;
using Stripe.Checkout;
using TraffiLearn.Application.Abstractions.Payments;
using TraffiLearn.Infrastructure.Services.Payments.Options;

namespace TraffiLearn.Infrastructure.Services.Payments
{
    internal sealed class StripePaymentService : IPaymentService
    {
        private readonly StripeSettings _settings;

        public StripePaymentService(IOptions<StripeSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<Uri> CreateCheckoutSessionAsync(
            CreateCheckoutSessionRequest request,
            Dictionary<string, string>? metadata = default)
        {
            var sessionOptions = BuildSessionOptions(request, metadata);

            var session = await CreateSessionAsync(sessionOptions);

            return new Uri(session.Url);
        }

        private SessionCreateOptions BuildSessionOptions(
            CreateCheckoutSessionRequest request,
            Dictionary<string, string>? metadata)
        {
            return new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = BuildLineItems(request),
                Mode = request.PaymentMode,
                SuccessUrl = _settings.SuccessUrl,
                CancelUrl = _settings.CancelUrl,
                Metadata = metadata
            };
        }

        private List<SessionLineItemOptions> BuildLineItems(CreateCheckoutSessionRequest request)
        {
            return new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmountDecimal = request.Amount * 100,
                        Currency = request.Currency.ToLower(),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = request.ProductName
                        }
                    },
                    Quantity = request.Quantity
                }
            };
        }

        private async Task<Session> CreateSessionAsync(SessionCreateOptions sessionOptions)
        {
            var service = new SessionService();

            return await service.CreateAsync(sessionOptions);
        }
    }
}
