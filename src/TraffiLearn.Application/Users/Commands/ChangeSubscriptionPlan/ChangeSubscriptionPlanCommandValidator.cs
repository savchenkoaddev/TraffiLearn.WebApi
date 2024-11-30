using FluentValidation;

namespace TraffiLearn.Application.Users.Commands.ChangeSubscriptionPlan
{
    internal sealed class ChangeSubscriptionPlanCommandValidator
        : AbstractValidator<ChangeSubscriptionPlanCommand>
    {
        public ChangeSubscriptionPlanCommandValidator()
        {
            RuleFor(x => x.SubscriptionPlanId)
                .NotEmpty();
        }
    }
}
