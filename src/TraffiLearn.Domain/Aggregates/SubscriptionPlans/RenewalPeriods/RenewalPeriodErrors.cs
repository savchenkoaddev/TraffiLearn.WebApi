using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.SubscriptionPlans.RenewalPeriods
{
    public static class RenewalPeriodErrors
    {
        public static readonly Error NegativeInterval =
            Error.Validation(
                code: "RenewalPeriod.NegativeInterval",
                description: "Interval cannot be less than zero.");
    }
}
