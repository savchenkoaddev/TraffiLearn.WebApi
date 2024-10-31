using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Directories.DirectorySections.SectionNames
{
    public static class SectionNameErrors
    {
        public static readonly Error Empty =
           Error.Validation(
               code: "SectionName.Empty",
               description: "Section name cannot be empty.");

        public static Error TooLong(int allowedLength) =>
            Error.Validation(
                code: "SectionName.TooLong",
                description: $"Section name must not exceed {allowedLength} characters.");
    }
}
