using TraffiLearn.Domain.Aggregates.Directories.Errors.Sections;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Directories.ValueObjects.Sections
{
    public sealed class SectionName : ValueObject
    {
        public const int MaxLength = 200;

        private SectionName(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<SectionName> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<SectionName>(
                    SectionNameErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<SectionName>(
                    SectionNameErrors.TooLong(allowedLength: MaxLength));
            }

            return new SectionName(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
