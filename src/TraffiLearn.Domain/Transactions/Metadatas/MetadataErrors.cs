using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Transactions.Metadatas
{
    public static class MetadataErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "Metadata.Empty",
                description: "Metadata content cannot be empty.");

        public static Error TooLong(int maxLength) =>
            Error.Validation(
                code: "Metadata.TooLong",
                description: $"Metadata content must not exceed {maxLength} characters.");
    }
}
