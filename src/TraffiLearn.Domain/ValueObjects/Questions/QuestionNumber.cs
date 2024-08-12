using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.ValueObjects.Questions
{
    public sealed class QuestionNumber : ValueObject
    {
        public const int MinValue = 1;

        private QuestionNumber(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public static Result<QuestionNumber> Create(int value)
        {
            if (value < MinValue)
            {
                return Result.Failure<QuestionNumber>(
                    QuestionNumberErrors.TooSmall(minValue: MinValue));
            }

            return new QuestionNumber(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
