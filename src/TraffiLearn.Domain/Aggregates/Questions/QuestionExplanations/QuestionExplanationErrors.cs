using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Questions.QuestionExplanations
{
    public static class QuestionExplanationErrors
    {
        public static readonly Error EmptyText =
            Error.Validation(
                code: "QuestionExplanation.EmptyText",
                description: "Question explanation cannot be empty.");

        public static Error TooLongText(int allowedLength) =>
            Error.Validation(
                code: "QuestionExplanation.TooLongText",
                description: $"Question explanation text must not exceed {allowedLength} characters.");
    }
}
