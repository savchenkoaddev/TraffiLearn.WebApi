using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Directories.DirectoryNames
{
    public static class DirectoryNameErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "DirectoryName.Empty",
                description: "Directory name cannot be empty.");

        public static Error TooLong(int allowedLength) =>
            Error.Validation(
                code: "DirectoryName.TooLong",
                description: $"Directory name must not exceed {allowedLength} characters.");
    }
}
