using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Webhooks.Stripe
{
    public sealed record HandleStripeWebhookCommand : IRequest<Result>;
}
