using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.ValueObjects
{
    public sealed class Answer : ValueObject
    {
        private const int MaxTextLength = 300;

        private Answer(
            string text,
            bool isCorrect)
        {
            Text = text;
            IsCorrect = isCorrect;
        }

        public string Text { get; init; }

        public bool IsCorrect { get; init; }

        public static Answer Create(
            string text,
            bool isCorrect)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Answer text cannot be empty.");
            }

            if (text.Length > MaxTextLength)
            {
                throw new ArgumentException($"Answer text must not exceed {MaxTextLength} characters.");
            }

            return new Answer(
                text,
                isCorrect);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            //Compare answers by text only
            yield return Text;
        }
    };
}
