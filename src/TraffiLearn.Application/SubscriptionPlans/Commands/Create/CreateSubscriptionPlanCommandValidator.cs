using FluentValidation;
using TraffiLearn.Application.Common.DTO;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.Domain.SubscriptionPlans.PlanDescriptions;
using TraffiLearn.Domain.SubscriptionPlans.PlanFeatures;
using TraffiLearn.Domain.SubscriptionPlans.PlanTiers;

namespace TraffiLearn.Application.SubscriptionPlans.Commands.Create
{
    internal sealed class CreateSubscriptionPlanCommandValidator
        : AbstractValidator<CreateSubscriptionPlanCommand>
    {
        public CreateSubscriptionPlanCommandValidator()
        {
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
