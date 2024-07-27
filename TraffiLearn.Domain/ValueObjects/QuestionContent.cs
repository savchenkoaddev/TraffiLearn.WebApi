using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.ValueObjects
{
    public sealed class QuestionContent : ValueObject
    {
        public const int MaxLength = 2000;

        private QuestionContent(string value)
        {
            Value = value;
        }

        public string Value { get; init; }

        public static QuestionContent Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Question content cannot be empty.");
            }

            if (value.Length > MaxLength)
            {
                throw new ArgumentException($"Question content must not exceed {MaxLength} characters.");
            }

            return new QuestionContent(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
