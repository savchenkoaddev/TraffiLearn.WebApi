using Microsoft.Extensions.Options;
using Stripe;
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
            CreateCheckoutSessionRequest request)
        {
            var sessionOptions = BuildSessionOptions(request);

            var session = await CreateSessionAsync(sessionOptions);

            return new Uri(session.Url);
        }

        private SessionCreateOptions BuildSessionOptions(CreateCheckoutSessionRequest request)
        {
            return new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = BuildLineItems(request),
                Mode = "payment",
                SuccessUrl = _settings.SuccessUrl,
                CancelUrl = _settings.CancelUrl
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
                        UnitAmount = request.Amount,
                        Currency = request.Currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = request.ProductName
                        }
                    },
                    Quantity = 1
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
