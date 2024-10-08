using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Questions.ValueObjects
{
    public sealed class Answer : ValueObject
    {
        public const int MaxTextLength = 300;

        private Answer(
            string text,
            bool isCorrect)
        {
            Text = text;
            IsCorrect = isCorrect;
        }

        public string Text { get; }

        public bool IsCorrect { get; }

        public static Result<Answer> Create(
            string? text,
            bool isCorrect)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Result.Failure<Answer>(
                    AnswerErrors.EmptyText);
            }

            if (text.Length > MaxTextLength)
            {
                return Result.Failure<Answer>(
                    AnswerErrors.TooLongText(allowedLength: MaxTextLength));
            }

            return new Answer(
                text,
                isCorrect);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            // Compare answers by text only
            yield return Text;
        }
    };
}
