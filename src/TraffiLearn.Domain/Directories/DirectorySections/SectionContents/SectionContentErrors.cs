using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Directories.DirectorySections.SectionContents
{
    public static class SectionContentErrors
    {
        public static readonly Error Empty =
           Error.Validation(
               code: "SectionContent.Empty",
               description: "Section content cannot be empty.");

        public static Error TooLong(int allowedLength) =>
            Error.Validation(
                code: "SectionContent.TooLong",
                description: $"Section content must not exceed {allowedLength} characters.");
    }
}
