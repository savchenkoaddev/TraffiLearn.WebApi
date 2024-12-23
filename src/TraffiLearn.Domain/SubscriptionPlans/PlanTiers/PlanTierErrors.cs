﻿using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.SubscriptionPlans.PlanTiers
{
    public static class PlanTierErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "PlanTier.Empty",
                description: "Plan tier cannot be empty.");

        public static Error TooLong(int maxLength) =>
            Error.Validation(
                code: "PlanTier.TooLong",
                description: $"Plan tier exceeds {maxLength} characters.");
    }
}
