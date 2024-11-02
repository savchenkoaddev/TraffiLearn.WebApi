using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.ServiceCenters.LocationNames
{
    public sealed class LocationName : ValueObject
    {
        public const int MaxLength = 200;

        private LocationName(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<LocationName> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<LocationName>(
                    LocationNameErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<LocationName>(
                    LocationNameErrors.TooLong(allowedLength: MaxLength));
            }

            return new LocationName(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
