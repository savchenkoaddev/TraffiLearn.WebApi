using MediatR;
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
        private readonly ISender _sender;

        public HandleStripeWebhookCommandHandler(
            IStripeWebhookService stripeWebhookService, 
            ISender sender)
        {
            _stripeWebhookService = stripeWebhookService;
            _sender = sender;
        }

        public async Task<Result> Handle(
            HandleStripeWebhookCommand request,
            CancellationToken cancellationToken)
        {
            var stripeEvent = _stripeWebhookService.ValidateStripeEvent(
                json: request.Payload,
                signatureHeader: request.Signature);

            var eventType = stripeEvent.GetStripeEventType();

            if (eventType == StripeEventType.PaymentIntentSucceeded)
            {
                var paymentIntentSucceededEvent = new PaymentIntentSucceededEvent();

                await _sender.Send(paymentIntentSucceededEvent, cancellationToken);
            }

            return Result.Success();
        }
    }
}
