using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.SubscriptionPlans.RenewalPeriods
{
    public static class RenewalPeriodErrors
    {
        public static readonly Error NegativeInterval =
            Error.Validation(
                code: "RenewalPeriod.NegativeInterval",
                description: "Interval cannot be less than zero.");
    }
}
