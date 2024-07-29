using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Errors
{
    public static class QuestionContentErrors
    {
        public static readonly Error EmptyText = Error.Validation("QuestionContent.EmptyText", "Question content cannot be empty.");

        public static Error TooLongText(int allowedLength) => Error.Validation("QuestionContent.TooLongText", $"Question content text must not exceed {allowedLength} characters.");
    }
}
