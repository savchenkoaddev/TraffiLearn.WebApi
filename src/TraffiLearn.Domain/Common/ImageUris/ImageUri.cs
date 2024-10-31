using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Common.ImageUris
{
    public sealed class ImageUri : ValueObject
    {
        public const int MaxLength = 300;

        private ImageUri(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<ImageUri> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<ImageUri>(
                    ImageUriErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<ImageUri>(
                    ImageUriErrors.TooLongText(allowedLength: MaxLength));
            }

            if (!IsValidUri(value))
            {
                return Result.Failure<ImageUri>(
                    ImageUriErrors.InvalidUri);
            }

            return new ImageUri(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        private static bool IsValidUri(string uriString)
        {
            if (Uri.TryCreate(uriString, UriKind.Absolute, out Uri uriResult))
            {
                return uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps;
            }

            return false;
        }
    }
}
