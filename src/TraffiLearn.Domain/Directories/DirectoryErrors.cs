using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Directories
{
    public static class DirectoryErrors
    {
        public static readonly Error EmptySections =
            Error.Validation(
                code: "Directory.EmptySections",
                description: "Sections cannot be empty.");

        public static Error TooManySections(int allowedSectionCount) =>
            Error.Validation(
                code: "Directory.TooManySections",
                description: $"Sections count cannot exceed {allowedSectionCount}.");

        public static readonly Error NotFound =
            Error.NotFound(
                code: "Directory.NotFound",
                description: "Directory has not been found.");
    }
}