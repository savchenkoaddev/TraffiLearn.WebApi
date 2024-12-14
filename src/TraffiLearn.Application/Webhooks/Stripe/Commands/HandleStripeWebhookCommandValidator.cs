using FluentValidation;

namespace TraffiLearn.Application.Webhooks.Stripe.Commands
{
    internal sealed class HandleStripeWebhookCommandValidator
        : AbstractValidator<HandleStripeWebhookCommand>
    {
        public HandleStripeWebhookCommandValidator()
        {
            RuleFor(x => x.Payload)
                .NotEmpty().WithMessage("Payload is required.");

            RuleFor(x => x.Signature)
                .NotEmpty().WithMessage("Signature is required.");
        }
    }
}
