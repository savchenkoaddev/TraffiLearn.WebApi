using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Webhooks.Stripe.Commands
{
    internal sealed class HandleStripeWebhookCommandHandler
        : IRequestHandler<HandleStripeWebhookCommand, Result>
    {
        private

        public Task<Result> Handle(
            HandleStripeWebhookCommand request,
            CancellationToken cancellationToken)
        {
            request.
        }
    }
}
