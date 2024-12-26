using TraffiLearn.Domain.SubscriptionPlans.RenewalPeriods;

namespace TraffiLearn.Application.UseCases.SubscriptionPlans.DTO
{
    public sealed record RenewalPeriodRequest(
        int Interval,
        RenewalPeriodType Type);
}
