using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.SubscriptionPlans.Commands.Update;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.Domain.SubscriptionPlans.PlanDescriptions;
using TraffiLearn.Domain.SubscriptionPlans.PlanFeatures;
using TraffiLearn.Domain.SubscriptionPlans.PlanTiers;
using TraffiLearn.Domain.SubscriptionPlans.RenewalPeriods;
using TraffiLearn.SharedKernel.Shared;
using TraffiLearn.SharedKernel.ValueObjects.Prices;

namespace TraffiLearn.Application.SubscriptionPlans.Mappers
{
    internal sealed class UpdateSubscriptionPlanCommandMapper
        : Mapper<UpdateSubscriptionPlanCommand, Result<SubscriptionPlan>>
    {
        public override Result<SubscriptionPlan> Map(UpdateSubscriptionPlanCommand source)
        {
            SubscriptionPlanId id = new(source.SubscriptionPlanId.Value);

            var tierResult = PlanTier.Create(source.Tier);

            if (tierResult.IsFailure)
            {
                return Result.Failure<SubscriptionPlan>(tierResult.Error);
            }

            var tier = tierResult.Value;

            var descriptionResult = PlanDescription.Create(source.Description);

            if (descriptionResult.IsFailure)
            {
                return Result.Failure<SubscriptionPlan>(descriptionResult.Error);
            }

            var description = descriptionResult.Value;

            var priceResult = Price.Create(
                amount: source.Price.Amount.Value,
                currency: source.Price.Currency.Value);

            if (priceResult.IsFailure)
            {
                return Result.Failure<SubscriptionPlan>(priceResult.Error);
            }

            var price = priceResult.Value;

            var renewalPeriodResult = RenewalPeriod.Create(
                interval: source.RenewalPeriod.Interval.Value,
                type: source.RenewalPeriod.Type.Value);

            if (renewalPeriodResult.IsFailure)
            {
                return Result.Failure<SubscriptionPlan>(renewalPeriodResult.Error);
            }

            var renewalPeriod = renewalPeriodResult.Value;

            var featuresResult = ParseFeatures(source.Features);

            if (featuresResult.IsFailure)
            {
                return Result.Failure<SubscriptionPlan>(featuresResult.Error);
            }

            var features = featuresResult.Value;

            return SubscriptionPlan.Create(
                id: id,
                tier: tier,
                description: description,
                price: price,
                renewalPeriod: renewalPeriod,
                features: features);
        }

        private Result<List<PlanFeature>> ParseFeatures(IEnumerable<string> features)
        {
            var featuresList = new List<PlanFeature>();

            foreach (var feature in features)
            {
                var featureResult = PlanFeature.Create(feature);

                if (featureResult.IsFailure)
                {
                    return Result.Failure<List<PlanFeature>>(featureResult.Error);
                }

                featuresList.Add(featureResult.Value);
            }

            return featuresList;
        }
    }
}
