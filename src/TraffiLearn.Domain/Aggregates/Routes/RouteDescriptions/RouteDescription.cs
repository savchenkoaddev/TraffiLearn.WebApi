using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Routes.RouteDescriptions
{
    public sealed class RouteDescription : ValueObject
    {
        public const int MaxLength = 2000;

        private RouteDescription(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<RouteDescription> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<RouteDescription>(
                    RouteDescriptionErrors.EmptyText);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<RouteDescription>(
                    RouteDescriptionErrors.TooLongText(allowedLength: MaxLength));
            }

            return new RouteDescription(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
