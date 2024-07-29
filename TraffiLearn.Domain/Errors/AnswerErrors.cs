using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Errors
{
    public static class AnswerErrors
    {
        public static readonly Error EmptyText = Error.Validation("Answer.EmptyText", "Answer text cannot be empty.");

        public static Error TooLongText(int allowedLength) => Error.Validation("Answer.TooLongText", $"Answer text must not exceed {allowedLength} characters.");
    }
}
