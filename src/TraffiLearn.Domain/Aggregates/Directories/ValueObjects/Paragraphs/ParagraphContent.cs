using TraffiLearn.Domain.Aggregates.Directories.Errors.Paragraphs;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Directories.ValueObjects.Paragraphs
{
    public sealed class ParagraphContent : ValueObject
    {
        public const int MaxLength = 2000;

        private ParagraphContent(string value)
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
