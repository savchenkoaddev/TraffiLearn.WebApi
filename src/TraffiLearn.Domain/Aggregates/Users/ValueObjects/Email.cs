using System.Text.RegularExpressions;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Users.ValueObjects
{
    public sealed class Email : ValueObject
    {
        public const int MaxLength = 254;

        private Email(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<Email> Create(string? value)
        {
            var validationResult = ValidateEmailString(value);

            if (validationResult.IsFailure)
            {
                return Result.Failure<Email>(validationResult.Error);
            }

            return new Email(value!);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        private static Result ValidateEmailString(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return EmailErrors.Empty;
            }

            if (email.Length > MaxLength)
            {
                return EmailErrors.TooLong(
                    maxLength: MaxLength);
            }

            var emailRegex = new Regex(
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase);

            if (!emailRegex.IsMatch(email))
            {
                return EmailErrors.InvalidFormat;
            }

            return Result.Success();
        }
    }
}
