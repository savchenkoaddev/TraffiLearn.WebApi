using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Errors.Users
{
    public static class UserErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "User.NotFound",
                description: "User has not been found.");

        public static readonly Error AlreadyRegistered =
            Error.Validation(
                code: "User.AlreadyRegistered",
                description: "The same user has already been registered.");

        public static readonly Error InvalidCredentials =
            Error.OperationFailure(
                code: "User.InvalidCredentials",
                description: "The provided credentials are invalid.");

        public static readonly Error CannotLogin =
            Error.OperationFailure(
                code: "User.CannotLogin",
                description: "The user cannot login due to some reasons. Check if the email is confirmed.");

        public static readonly Error CommentAlreadyAdded =
            Error.Validation(
                code: "User.CommentAlreadyAdded",
                description: "The user already has the same comment.");

        public static readonly Error QuestionNotFound =
            Error.Validation(
                code: "User.QuestionNotFound",
                description: "The question has not been found.");

        public static readonly Error QuestionAlreadyMarked =
            Error.Validation(
                code: "User.QuestionAlreadyMarked",
                description: "The same question has already been marked.");

        public static readonly Error QuestionAlreadyUnmarked =
            Error.Validation(
                code: "User.QuestionAlreadyUnmarked",
                description: "The same question has already been unmarked.");

        public static readonly Error QuestionAlreadyLikedByUser =
           Error.Validation(
               code: "User.QuestionAlreadyLikedByUser",
               description: "The question is already liked by the user.");

        public static readonly Error QuestionAlreadyDislikedByUser =
            Error.Validation(
                code: "User.QuestionAlreadyDislikedByUser",
                description: "The question is already disliked by the user.");

        public static readonly Error QuestionNotLiked =
            Error.Validation(
                code: "User.QuestionNotLiked",
                description: "The question is not liked by the user.");

        public static readonly Error QuestionNotDisliked =
            Error.Validation(
                code: "User.QuestionNotDisliked",
                description: "The question is not disliked by the user.");

        public static readonly Error CantDislikeQuestionIfLiked =
            Error.Validation(
                code: "User.CantDislikeQuestionIfLiked",
                description: "Unable to dislike the question, because the question has already been liked.");

        public static readonly Error CantLikeQuestionIfDisliked =
            Error.Validation(
                code: "User.CantLikeQuestionIfDisliked",
                description: "Unable to like the question, because the question has already been disliked.");
    }
}
