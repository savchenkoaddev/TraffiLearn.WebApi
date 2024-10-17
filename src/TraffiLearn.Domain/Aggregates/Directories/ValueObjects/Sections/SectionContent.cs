using TraffiLearn.Domain.Aggregates.Directories.Errors.Sections;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Directories.ValueObjects.Sections
{
    public sealed class SectionContent : ValueObject
    {
        public const int MaxLength = 10000;

        private SectionContent(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<SectionContent> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<SectionContent>(
                    SectionContentErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<SectionContent>(
                    SectionContentErrors.TooLong(allowedLength: MaxLength));
            }

            return new SectionContent(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
