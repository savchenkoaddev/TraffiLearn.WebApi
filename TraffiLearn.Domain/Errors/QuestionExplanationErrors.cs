using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Errors
{
    public static class QuestionExplanationErrors
    {
        public static readonly Error EmptyText = Error.Validation("QuestionExplanation.EmptyText", "Question explanation cannot be empty.");

        public static Error TooLongText(int allowedLength) => Error.Validation("QuestionExplanation.TooLongText", $"Question explanation text must not exceed {allowedLength} characters.");
    }
}
