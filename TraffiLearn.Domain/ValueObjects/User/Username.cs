using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.ValueObjects.User
{
    public sealed class Username : ValueObject
    {
        private Username(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<Username> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<Username>(UsernameErrors.Empty);
            }

            return new Username(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
