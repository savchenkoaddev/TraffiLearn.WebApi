using TraffiLearn.Domain.Aggregates.Directories.Errors.Directories;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Directories.ValueObjects.Directories
{
    public sealed class DirectoryName : ValueObject
    {
        public const int MaxLength = 200;

        private DirectoryName(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<DirectoryName> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<DirectoryName>(
                    DirectoryNameErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<DirectoryName>(
                    DirectoryNameErrors.TooLong(allowedLength: MaxLength));
            }

            return new DirectoryName(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
