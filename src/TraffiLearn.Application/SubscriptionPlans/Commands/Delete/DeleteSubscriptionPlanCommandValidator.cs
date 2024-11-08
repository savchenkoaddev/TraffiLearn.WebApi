using FluentValidation;

namespace TraffiLearn.Application.SubscriptionPlans.Commands.Delete
{
    internal sealed class DeleteSubscriptionPlanCommandValidator
        : AbstractValidator<DeleteSubscriptionPlanCommand>
    {
        public DeleteSubscriptionPlanCommandValidator()
        {
            RuleFor(x => x.SubscriptionPlanId)
                .NotEmpty();
        }
    }
}
