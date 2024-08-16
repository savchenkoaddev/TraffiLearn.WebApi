using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Comments.Errors
{
    public static class CommentContentErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "CommentContent.Empty",
                description: "Comment content cannot be empty.");

        public static Error TooLong(int maxLength) =>
            Error.Validation(
                code: "CommentContent.TooLong",
                description: $"Comment content exceeds {maxLength} characters.");
    }
}
