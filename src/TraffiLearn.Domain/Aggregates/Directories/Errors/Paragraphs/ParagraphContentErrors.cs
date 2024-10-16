using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Directories.Errors.Paragraphs
{
    public static class ParagraphContentErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "ParagraphContent.Empty",
                description: "Paragraph content cannot be empty.");

        public static Error TooLong(int allowedLength) =>
            Error.Validation(
                code: "ParagraphContent.TooLong",
                description: $"Paragraph content must not exceed {allowedLength} characters.");
    }
}
