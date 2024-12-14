using FluentValidation;

namespace TraffiLearn.Application.UseCases.Users.Commands.RequestChangeSubscriptionPlan
{
    internal sealed class RequestChangeSubscriptionPlanCommandValidator
        : AbstractValidator<RequestChangeSubscriptionPlanCommand>
    {
        public RequestChangeSubscriptionPlanCommandValidator()
        {
            RuleFor(x => x.SubscriptionPlanId)
                .NotEmpty();
        }
    }
}
