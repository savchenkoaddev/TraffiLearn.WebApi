﻿using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.SubscriptionPlans
{
    public static class SubscriptionPlanErrors
    {
        public static readonly Error EmptyFeatures =
            Error.Validation(
                code: "SubscriptionPlan.EmptyFeatures",
                description: "Subscription plan should have at least one feature.");

        public static Error TooMuchFeatures(int maxFeaturesCount) =>
            Error.Validation(
                code: "SubscriptionPlan.TooMuchFeatures",
                description: $"Subscription plan can't have more than {maxFeaturesCount} features.");
    }
}
