using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Common.DTO;
using TraffiLearn.Application.SubscriptionPlans.DTO;
using TraffiLearn.Domain.SubscriptionPlans;

namespace TraffiLearn.Application.SubscriptionPlans.Mappers
{
    internal sealed class SubscriptionPlanEntityToResponseMapper
        : Mapper<SubscriptionPlan, SubscriptionPlanResponse>
    {
        public override SubscriptionPlanResponse Map(SubscriptionPlan source)
        {
            var price = new PriceResponse(
                Amount: source.Price.Amount,
                Currency: source.Price.Currency.ToString());

            var renewalPeriod = new RenewalPeriodResponse(
                Interval: source.RenewalPeriod.Interval,
                Type: source.RenewalPeriod.Type.ToString());

            var features = source.Features
                .Select(feature => feature.Value)
                .ToList();

            return new SubscriptionPlanResponse(
                Id: source.Id.Value,
                Tier: source.Tier.Value,
                Description: source.Description.Value,
                Price: price,
                RenewalPeriod: renewalPeriod,
                Features: features);
        }
    }
}
