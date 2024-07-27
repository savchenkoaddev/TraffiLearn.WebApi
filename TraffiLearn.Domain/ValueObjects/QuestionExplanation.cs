using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.ValueObjects
{
    public sealed class QuestionExplanation : ValueObject
    {
        public const int MaxLength = 2000;

        private QuestionExplanation(string value)
        {
            Value = value;
        }

        public string Value { get; init; }

        public static QuestionExplanation Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Question explanation cannot be empty.");
            }

            if (value.Length > MaxLength)
            {
                throw new ArgumentException($"Question explanation must not exceed {MaxLength} characters.");
            }

            return new QuestionExplanation(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
