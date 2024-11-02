using FluentValidation;

namespace TraffiLearn.Application.SubscriptionPlans.Queries.GetById
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
