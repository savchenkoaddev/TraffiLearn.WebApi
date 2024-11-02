using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.SubscriptionPlans
{
    public static class SubscriptionPlanErrors
    {
        public static readonly Error EmptyFeatures =
            Error.Validation(
                code: "SubscriptionPlan.EmptyFeatures",
                description: "Subscription plan should have at least one feature.");

        public static readonly Error NotFound =
            Error.Validation(
                code: "SubscriptionPlan.NotFound",
                description: "A subscription plan with the provided id has not been found.");

        public static Error TooMuchFeatures(int maxFeaturesCount) =>
            Error.Validation(
                code: "SubscriptionPlan.TooMuchFeatures",
                description: $"Subscription plan can't have more than {maxFeaturesCount} features.");
    }
}
