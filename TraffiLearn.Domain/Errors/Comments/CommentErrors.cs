using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Errors.Comments
{
    public static class CommentErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "Comment.QuestionNotFound",
                description: "Question has not been found.");
    }
}
