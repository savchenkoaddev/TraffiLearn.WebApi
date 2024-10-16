using TraffiLearn.Domain.Aggregates.Directories.Errors.Paragraphs;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Directories.ValueObjects.Paragraphs
{
    public sealed class ParagraphNumber : ValueObject
    {
        public const int MaxLength = 30;

        private ParagraphNumber(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<ParagraphNumber> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<ParagraphNumber>(
                    ParagraphNumberErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<ParagraphNumber>(
                    ParagraphNumberErrors.TooLong(allowedLength: MaxLength));
            }

            return new ParagraphNumber(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
