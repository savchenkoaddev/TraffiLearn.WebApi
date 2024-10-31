using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Directories.DirectorySections
{
    public static class DirectorySectionErrors
    {
        public static readonly Error EmptyParagraphs =
            Error.Validation(
                code: "DirectorySection.EmptyParagraphs",
                description: "Paragraphs cannot be empty.");
    }
}
