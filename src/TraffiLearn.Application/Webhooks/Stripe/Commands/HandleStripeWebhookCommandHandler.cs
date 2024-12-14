using MediatR;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;
using TraffiLearn.Application.Abstractions.Payments;
using TraffiLearn.Application.Webhooks.Stripe.Events;
using TraffiLearn.Application.Webhooks.Stripe.Extensions;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Webhooks.Stripe.Commands
{
    internal sealed class HandleStripeWebhookCommandHandler
        : IRequestHandler<HandleStripeWebhookCommand, Result>
    {
        private readonly IStripeWebhookService _stripeWebhookService;
        private readonly IPublisher _publisher;
        private readonly ILogger<HandleStripeWebhookCommandHandler> _logger;

        public HandleStripeWebhookCommandHandler(
            IStripeWebhookService stripeWebhookService,
            IPublisher publisher,
            ILogger<HandleStripeWebhookCommandHandler> logger)
        {
            _stripeWebhookService = stripeWebhookService;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task<Result> Handle(
            HandleStripeWebhookCommand request,
            CancellationToken cancellationToken)
        {
            var stripeEvent = _stripeWebhookService.ValidateStripeEvent(
                json: request.Payload,
                signatureHeader: request.Signature);

            var eventType = stripeEvent.GetStripeEventType();

            if (eventType == StripeEventType.CheckoutSessionCompleted)
            {
                (Guid SubscriptionPlanId, Guid UserId) ids = ExtractMetadata(stripeEvent);

                var checkoutSessionCompletedEvent = new CheckoutSessionCompletedEvent(
                    SubscriptionPlanId: ids.SubscriptionPlanId,
                    UserId: ids.UserId);

                await _publisher.Publish(checkoutSessionCompletedEvent, cancellationToken);
            }

            return Result.Success();
        }

        private static (Guid SubscriptionPlanId, Guid UserId) ExtractMetadata(
            Event stripeEvent)
        {
            var session = stripeEvent.Data.Object as Session;

            if (session is null)
            {
                throw new InvalidOperationException(
                    "Session is null in Stripe webhook event");
            }

            var subscriptionPlanIdString = session.Metadata["subscriptionPlanId"];
            var userIdString = session.Metadata["userId"];

            if (Guid.TryParse(subscriptionPlanIdString, out var subscriptionPlanId) &&
                Guid.TryParse(userIdString, out var userId))
            {
                return (subscriptionPlanId, userId);
            }

            throw new InvalidOperationException(
                "Ids from the metadata are not of type Guid.");
        }
    }
}
