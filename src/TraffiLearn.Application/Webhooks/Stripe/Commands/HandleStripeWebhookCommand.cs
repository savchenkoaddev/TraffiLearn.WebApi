using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Webhooks.Stripe.Commands
{
    public sealed record HandleStripeWebhookCommand(
        string Payload,
        string Signature) : IRequest<Result>;
}
