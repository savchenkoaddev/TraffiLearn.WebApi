using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Routes.RouteNumbers
{
    public sealed class RouteNumber : ValueObject
    {
        public const int MinValue = 1;

        private RouteNumber(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public static Result<RouteNumber> Create(int value)
        {
            if (value < MinValue)
            {
                return Result.Failure<RouteNumber>(
                    RouteNumberErrors.TooSmall(minValue: MinValue));
            }

            return new RouteNumber(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
