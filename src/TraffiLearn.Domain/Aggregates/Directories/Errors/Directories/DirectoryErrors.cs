using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Directories.Errors.Directories
{
    public static class DirectoryErrors
    {
        public static readonly Error EmptySections =
            Error.Validation(
                code: "Directory.EmptySections",
                description: "Sections cannot be empty.");

        public static readonly Error TooManySections(int allowedSectionCount) =>
            Error.Validation(
                code: "Directory.TooManySections",
                description: $"Sections count cannot exceed {allowedSectionCount}.");
    }
}
