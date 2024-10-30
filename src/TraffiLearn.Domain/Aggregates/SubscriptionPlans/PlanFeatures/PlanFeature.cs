using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.SubscriptionPlans.PlanFeatures
{
    public sealed class PlanFeature : ValueObject
    {
        public const int MaxLength = 200;

        private PlanFeature(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<PlanFeature> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<PlanFeature>(
                    PlanFeatureErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<PlanFeature>(
                    PlanFeatureErrors.TooLong(MaxLength));
            }

            return new PlanFeature(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
