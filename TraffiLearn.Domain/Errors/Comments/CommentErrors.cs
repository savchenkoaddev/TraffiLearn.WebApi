using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Errors.Comments
{
    public static class CommentErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "Comment.QuestionNotFound",
                description: "Question has not been found.");

        public static readonly Error CommentAlreadyAdded =
            Error.Validation(
                code: "Comment.CommentAlreadyAdded",
                description: "The same comment has already been added within the root comment.");
    }
}
