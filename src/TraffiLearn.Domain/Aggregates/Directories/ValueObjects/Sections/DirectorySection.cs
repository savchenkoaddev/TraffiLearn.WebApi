using TraffiLearn.Domain.Aggregates.Directories.Errors.Sections;
using TraffiLearn.Domain.Aggregates.Directories.ValueObjects.Paragraphs;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Directories.ValueObjects.Sections
{
    public sealed class DirectorySection : ValueObject
    {
        private DirectorySection(
            List<SectionParagraph> paragraphs)
        {
            Paragraphs = paragraphs;
        }

        public List<SectionParagraph> Paragraphs { get; }

        public static Result<DirectorySection> Create(
            List<SectionParagraph> paragraphs)
        {
            ArgumentNullException.ThrowIfNull(paragraphs);

            if (paragraphs.Count == 0)
            {
                return Result.Failure<DirectorySection>(DirectorySectionErrors.EmptyParagraphs);
            }

            return new DirectorySection(paragraphs);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Paragraphs;
        }
    }
}
