using TraffiLearn.Application.Common.DTO;

namespace TraffiLearn.Application.SubscriptionPlans.DTO
{
    public sealed record SubscriptionPlanResponse(
        Guid Id,
        string Tier,
        string Description,
        PriceResponse Price,
        RenewalPeriodResponse RenewalPeriod,
        List<string> Features);
}
