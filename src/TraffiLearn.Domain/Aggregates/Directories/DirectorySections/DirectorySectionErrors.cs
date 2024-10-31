using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Directories.DirectorySections
{
    public static class DirectorySectionErrors
    {
        public static readonly Error EmptyParagraphs =
            Error.Validation(
                code: "DirectorySection.EmptyParagraphs",
                description: "Paragraphs cannot be empty.");
    }
}
