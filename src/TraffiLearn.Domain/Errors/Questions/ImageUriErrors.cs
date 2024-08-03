using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Errors.Questions
{
    public static class ImageUriErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "ImageUri.EmptyText",
                description: "Image uri cannot be empty.");

        public static readonly Error InvalidUri =
            Error.Validation(
                code: "ImageUri.InvalidUri",
                description: "Provided image uri is invalid.");

        public static Error TooLongText(int allowedLength) =>
            Error.Validation(
                code: "ImageUri.TooLongText",
                description: $"Image uri text must not exceed {allowedLength} characters.");
    }
}
