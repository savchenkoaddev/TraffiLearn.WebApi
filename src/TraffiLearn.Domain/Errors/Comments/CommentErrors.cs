using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Errors.Comments
{
    public static class CommentErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "Comment.NotFound",
                description: "Comment has not been found.");

        public static readonly Error CommentAlreadyAdded =
            Error.Validation(
                code: "Comment.CommentAlreadyAdded",
                description: "The same comment has already been added within the root comment.");

        public static readonly Error AlreadyLikedByUser =
            Error.Validation(
                code: "Comment.AlreadyLikedByUser",
                description: "The comment is already liked by the user.");

        public static readonly Error AlreadyDislikedByUser =
            Error.Validation(
                code: "Comment.AlreadyDislikedByUser",
                description: "The comment is already disliked by the user.");

        public static readonly Error NotLikedByUser =
            Error.Validation(
                code: "Comment.NotLikedByUser",
                description: "The comment is not liked by the user.");

        public static readonly Error NotDislikedByUser =
            Error.Validation(
                code: "Comment.NotDislikedByUser",
                description: "The comment is not disliked by the user.");

        public static readonly Error CantDislikeIfLikedByUser =
            Error.Validation(
                code: "Comment.CantDislikeIfLikedByUser",
                description: "Unable to dislike the comment, because the comment has already been liked by the user.");

        public static readonly Error CantLikeIfDislikedByUser =
            Error.Validation(
                code: "Comment.CantLikeIfDislikedByUser",
                description: "Unable to like the comment, because the comment has already been disliked by the user.");
    }
}
