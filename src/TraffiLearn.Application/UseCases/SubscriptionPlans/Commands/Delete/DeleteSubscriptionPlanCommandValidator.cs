using FluentValidation;

namespace TraffiLearn.Application.UseCases.SubscriptionPlans.Commands.Delete
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
