using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.SubscriptionPlans.PlanFeatures
{
    public static class PlanFeatureErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "PlanFeature.Empty",
                description: "Plan feature cannot be empty.");

        public static Error TooLong(int maxLength) =>
            Error.Validation(
                code: "PlanFeature.TooLong",
                description: $"Plan feature exceeds {maxLength} characters.");
    }
}