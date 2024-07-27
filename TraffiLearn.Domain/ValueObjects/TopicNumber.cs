using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.ValueObjects
{
    public sealed class TopicNumber : ValueObject
    {
        public const int MinValue = 1;

        private TopicNumber(int value)
        {
            Value = value;
        }

        public int Value { get; init; }

        public static TopicNumber Create(int value)
        {
            if (value < MinValue)
            {
                throw new ArgumentException($"Topic number cannot be less than {MinValue} characters.");
            }

            return new TopicNumber(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
