using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.ValueObjects
{
    public sealed class ImageUri : ValueObject
    {
        public const int MaxLength = 300;

        private ImageUri(string value)
        {
            Value = value;
        }

        public string Value { get; init; }

        public static ImageUri Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Image uri cannot be empty.");
            }

            if (value.Length > MaxLength)
            {
                throw new ArgumentException($"Image uri must not exceed {MaxLength} characters.");
            }

            if (!IsValidUri(value))
            {
                throw new ArgumentException("Image uri is not a valid URI.");
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
