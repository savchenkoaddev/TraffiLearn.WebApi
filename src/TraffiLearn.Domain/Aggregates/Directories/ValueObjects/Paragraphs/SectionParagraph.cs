using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Directories.ValueObjects.Paragraphs
{
    public sealed class SectionParagraph : ValueObject
    {
        private SectionParagraph(
            ParagraphNumber number,
            ParagraphContent content)
        {
            Number = number;
            Content = content;
        }

        public ParagraphNumber Number { get; }

        public ParagraphContent Content { get; }

        public static Result<SectionParagraph> Create(
            ParagraphNumber number,
            ParagraphContent content)
        {
            ArgumentNullException.ThrowIfNull(nameof(number));
            ArgumentNullException.ThrowIfNull(nameof(content));

            return new SectionParagraph(number, content);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Number;
            yield return Content;
        }
    }
}
