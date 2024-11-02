using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.SubscriptionPlans.PlanFeatures
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