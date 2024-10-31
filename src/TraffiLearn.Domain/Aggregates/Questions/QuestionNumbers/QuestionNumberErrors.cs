using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Questions.QuestionNumbers
{
    public static class QuestionNumberErrors
    {
        public static Error TooSmall(int minValue) =>
            Error.Validation(
                code: "QuestionNumber.TooSmall",
                description: $"Question number must be greater or equal to {minValue}");
    }
}
