﻿using TraffiLearn.Domain.SubscriptionPlans.PlanDescriptions;
using TraffiLearn.Domain.SubscriptionPlans.PlanFeatures;
using TraffiLearn.Domain.SubscriptionPlans.PlanTiers;
using TraffiLearn.Domain.SubscriptionPlans.RenewalPeriods;
using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;
using TraffiLearn.SharedKernel.ValueObjects.Prices;

namespace TraffiLearn.Domain.SubscriptionPlans
{
    public sealed class SubscriptionPlan : AggregateRoot<SubscriptionPlanId>
    {
        public const int MaxFeaturesCount = 20;

        private PlanTier _tier;
        private PlanDescription _description;
        private Price _price;
        private RenewalPeriod _renewalPeriod;
        private HashSet<PlanFeature> _features = [];

        private SubscriptionPlan()
            : base(new(Guid.Empty))
        { }

        private SubscriptionPlan(
            SubscriptionPlanId id,
            PlanTier tier,
            PlanDescription description,
            Price price,
            RenewalPeriod renewalPeriod,
            List<PlanFeature> features) : base(id)
        {
            Tier = tier;
            Description = description;
            Price = price;
            RenewalPeriod = renewalPeriod;
            _features = features.ToHashSet();
        }

        public PlanTier Tier
        {
            get
            {
                return _tier;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value);

                _tier = value;
            }
        }

        public PlanDescription Description
        {
            get
            {
                return _description;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value);

                _description = value;
            }
        }

        public Price Price
        {
            get
            {
                return _price;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value);

                _price = value;
            }
        }

        public RenewalPeriod RenewalPeriod
        {
            get
            {
                return _renewalPeriod;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value);

                _renewalPeriod = value;
            }
        }

        public IReadOnlyCollection<PlanFeature> Features => _features;

        public Result Update(
            PlanTier tier,
            PlanDescription description,
            Price price,
            RenewalPeriod renewalPeriod,
            List<PlanFeature> features)
        {
            ArgumentNullException.ThrowIfNull(features);

            var featuresValidationResult = ValidateFeatures(features);

            if (featuresValidationResult.IsFailure)
            {
                return Result.Failure(
                    featuresValidationResult.Error);
            }

            Tier = tier;
            Description = description;
            Price = price;
            RenewalPeriod = renewalPeriod;
            _features = features.ToHashSet();

            return Result.Success();
        }

        public static Result<SubscriptionPlan> Create(
            SubscriptionPlanId id,
            PlanTier tier,
            PlanDescription description,
            Price price,
            RenewalPeriod renewalPeriod,
            List<PlanFeature> features)
        {
            ArgumentNullException.ThrowIfNull(features);

            var featuresValidationResult = ValidateFeatures(features);

            if (featuresValidationResult.IsFailure)
            {
                return Result.Failure<SubscriptionPlan>(
                    featuresValidationResult.Error);
            }

            return new SubscriptionPlan(
                id,
                tier,
                description,
                price,
                renewalPeriod,
                features);
        }

        private static Result ValidateFeatures(List<PlanFeature> features)
        {
            if (features.Count == 0)
            {
                return SubscriptionPlanErrors.EmptyFeatures;
            }

            if (features.Count > MaxFeaturesCount)
            {
                return SubscriptionPlanErrors.TooMuchFeatures(MaxFeaturesCount);
            }

            return Result.Success();
        }
    }
}
