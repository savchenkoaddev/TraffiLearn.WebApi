using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.SubscriptionPlans.PlanDescriptions
{
    public sealed class PlanDescription : ValueObject
    {
        public const int MaxLength = 500;

        private PlanDescription(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<PlanDescription> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<PlanDescription>(PlanDescriptionErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<PlanDescription>(PlanDescriptionErrors.TooLong(
                    MaxLength));
            }

            return new PlanDescription(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
