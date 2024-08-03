using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.ValueObjects.Questions
{
    public sealed class QuestionContent : ValueObject
    {
        public const int MaxLength = 2000;

        private QuestionContent(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<QuestionContent> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<QuestionContent>(
                    QuestionContentErrors.EmptyText);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<QuestionContent>(
                    QuestionContentErrors.TooLongText(allowedLength: MaxLength));
            }

            return new QuestionContent(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
