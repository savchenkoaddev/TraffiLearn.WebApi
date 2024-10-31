using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.ServiceCenters.RoadNames
{
    public sealed class RoadName : ValueObject
    {
        public const int MaxLength = 200;

        private RoadName(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<RoadName> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<RoadName>(
                    RoadNameErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<RoadName>(
                    RoadNameErrors.TooLong(allowedLength: MaxLength));
            }

            return new RoadName(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
