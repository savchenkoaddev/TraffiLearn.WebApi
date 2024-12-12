using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Users.CancelationReasons
{
    public sealed class CancelationReason : ValueObject
    {
        public const int MaxLength = 200;

        private CancelationReason(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<CancelationReason> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<CancelationReason>(
                    CancelationReasonErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<CancelationReason>(
                    CancelationReasonErrors.TooLong(MaxLength));
            }

            return new CancelationReason(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
