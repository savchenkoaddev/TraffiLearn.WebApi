using FluentValidation;
using TraffiLearn.Application.Common.DTO;
using TraffiLearn.Domain.SubscriptionPlans.PlanDescriptions;
using TraffiLearn.Domain.SubscriptionPlans.PlanFeatures;
using TraffiLearn.Domain.SubscriptionPlans.PlanTiers;
using TraffiLearn.Domain.SubscriptionPlans;

namespace TraffiLearn.Application.SubscriptionPlans.Commands.Update
{
    internal sealed class UpdateSubscriptionPlanCommandValidator
        : AbstractValidator<UpdateSubscriptionPlanCommand>
    {
        public UpdateSubscriptionPlanCommandValidator()
        {
            RuleFor(x => x.SubscriptionPlanId)
                .NotEmpty();

            RuleFor(x => x.Tier)
                .NotEmpty()
                .MaximumLength(PlanTier.MaxLength);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(PlanDescription.MaxLength);

            RuleFor(x => x.Price)
                .NotEmpty()
                .SetValidator(new PriceRequestValidator() as IValidator<PriceRequest?>);

            RuleFor(x => x.RenewalPeriod)
                .NotEmpty()
                .ChildRules(x =>
                {
                    x.RuleFor(x => x.Interval)
                        .NotEmpty()
                        .GreaterThan(0);

                    x.RuleFor(x => x.Type)
                        .NotEmpty()
                        .IsInEnum().WithMessage("Type must be one of the supported types.");
                });

            RuleFor(x => x.Features)
                .NotEmpty()
                .Must(x => x.Count <= SubscriptionPlan.MaxFeaturesCount);

            RuleForEach(x => x.Features)
                .NotEmpty()
                .MaximumLength(PlanFeature.MaxTextLength);
        }
    }
}
