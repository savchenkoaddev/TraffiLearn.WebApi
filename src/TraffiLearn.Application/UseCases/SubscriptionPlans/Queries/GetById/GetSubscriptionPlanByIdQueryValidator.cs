using FluentValidation;

namespace TraffiLearn.Application.UseCases.SubscriptionPlans.Queries.GetById
{
    internal sealed class GetSubscriptionPlanByIdQueryValidator
        : AbstractValidator<GetSubscriptionPlanByIdQuery>
    {
        public GetSubscriptionPlanByIdQueryValidator()
        {
            RuleFor(x => x.SubscriptionPlanId)
                .NotEmpty();
        }
    }
}
