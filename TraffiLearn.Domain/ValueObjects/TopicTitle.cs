using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.ValueObjects
{
    public sealed class TopicTitle : ValueObject
    {
        public const int MaxLength = 300;

        private TopicTitle(string value)
        {
            Value = value;
        }

        public string Value { get; init; }

        public static TopicTitle Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Topic title cannot be empty.");
            }

            if (value.Length > MaxLength)
            {
                throw new ArgumentException($"Topic title length must not exceed {MaxLength} characters.");
            }

            return new TopicTitle(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
