using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Errors
{
    public static class ImageUriErrors
    {
        public static readonly Error Empty = Error.Validation("ImageUri.EmptyText", "Image uri cannot be empty.");

        public static readonly Error InvalidUri = Error.Validation("ImageUri.InvalidUri", "Provided image uri is invalid.");

        public static Error TooLongText(int allowedLength) => Error.Validation("ImageUri.TooLongText", $"Image uri text must not exceed {allowedLength} characters.");
    }
}
