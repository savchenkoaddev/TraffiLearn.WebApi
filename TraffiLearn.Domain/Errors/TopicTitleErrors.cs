using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Errors
{
    public static class TopicTitleErrors
    {
        public static readonly Error EmptyText = Error.Validation("TopicTitle.EmptyText", "Topic title cannot be empty.");

        public static Error TooLongText(int allowedLength) => Error.Validation("TopicTitle.TooLongText", $"Topic title text must not exceed {allowedLength} characters.");
    }
}
