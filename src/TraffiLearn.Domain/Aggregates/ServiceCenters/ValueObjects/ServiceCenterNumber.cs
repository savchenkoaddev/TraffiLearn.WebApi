using TraffiLearn.Domain.Aggregates.ServiceCenters.Errors;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.ServiceCenters.ValueObjects
{
    public sealed class ServiceCenterNumber : ValueObject
    {
        public const int MaxLength = 7;

        private ServiceCenterNumber(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<ServiceCenterNumber> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<ServiceCenterNumber>(
                    ServiceCenterNumberErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<ServiceCenterNumber>(
                    ServiceCenterNumberErrors.TooLong(
                        allowedLength: MaxLength));
            }

            if (int.TryParse(value, out _) == false)
            {
                return Result.Failure<ServiceCenterNumber>(
                    ServiceCenterNumberErrors.NotNumber);
            }

            return new ServiceCenterNumber(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
