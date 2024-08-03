using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Errors.Topics
{
    public static class TopicTitleErrors
    {
        public static readonly Error EmptyText = 
            Error.Validation(
                code: "TopicTitle.EmptyText", 
                description: "Topic title cannot be empty.");

        public static Error TooLongText(int allowedLength) => 
            Error.Validation(
                code: "TopicTitle.TooLongText", 
                description: $"Topic title text must not exceed {allowedLength} characters.");
    }
}
