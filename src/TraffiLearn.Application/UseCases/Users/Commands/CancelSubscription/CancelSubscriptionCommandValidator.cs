using FluentValidation;
using TraffiLearn.Application.Validators;
using TraffiLearn.Domain.Users.CancelationReasons;

namespace TraffiLearn.Application.UseCases.Users.Commands.CancelSubscription
{
    internal sealed class CancelSubscriptionCommandValidator
        : AbstractValidator<CancelSubscriptionCommand>
    {
        public CancelSubscriptionCommandValidator()
        {
            RuleFor(x => x.Reason)
               .MaximumLength(CancelationReason.MaxLength)
               .When(x => x.Reason is not null);
        }
    }
}
