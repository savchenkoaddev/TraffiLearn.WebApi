using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Questions.QuestionExplanations
{
    public sealed class QuestionExplanation : ValueObject
    {
        public const int MaxLength = 2000;

        private QuestionExplanation(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<QuestionExplanation> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<QuestionExplanation>(
                    QuestionExplanationErrors.EmptyText);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<QuestionExplanation>(
                    QuestionExplanationErrors.TooLongText(allowedLength: MaxLength));
            }

            return new QuestionExplanation(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
