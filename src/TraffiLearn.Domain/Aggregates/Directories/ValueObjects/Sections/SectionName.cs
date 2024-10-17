using TraffiLearn.Domain.Aggregates.Directories.Errors.Paragraphs;
using TraffiLearn.Domain.Aggregates.Directories.ValueObjects.Paragraphs;
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

        public static Result<ParagraphContent> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<ParagraphContent>(
                    ParagraphContentErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<ParagraphContent>(
                    ParagraphContentErrors.TooLong(allowedLength: MaxLength));
            }

            return new ParagraphContent(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
