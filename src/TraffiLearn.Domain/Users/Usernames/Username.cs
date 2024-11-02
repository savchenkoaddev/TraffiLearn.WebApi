using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Users.Usernames
{
    public sealed class Username : ValueObject
    {
        public const int MaxLength = 30;

        private Username(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<Username> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<Username>(UsernameErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<Username>(UsernameErrors.TooLong(
                    maxLength: MaxLength));
            }

            return new Username(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
