using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Directories.DirectoryNames
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
