using TraffiLearn.Domain.SubscriptionPlans.RenewalPeriods;

namespace TraffiLearn.Application.SubscriptionPlans.DTO
{
    public sealed record RenewalPeriodRequest(
        int? Interval,
        RenewalPeriodType? Type);
}
