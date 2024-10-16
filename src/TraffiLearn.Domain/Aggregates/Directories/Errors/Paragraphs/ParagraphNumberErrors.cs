using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Directories.Errors.Paragraphs
{
    public static class ParagraphNumberErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "ParagraphNumber.Empty",
                description: "Paragraph number cannot be empty.");

        public static Error TooLong(int allowedLength) =>
            Error.Validation(
                code: "ParagraphNumber.TooLong",
                description: $"Paragraph number must not exceed {allowedLength} characters.");
    }
}
