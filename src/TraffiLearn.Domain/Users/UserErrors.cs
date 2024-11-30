using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Users
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
                description: "The same user has already been registered. Use different email or username.");

        public static readonly Error EmailAlreadyTaken =
            Error.Validation(
                code: "User.EmailAlreadyTaken",
                description: "The same email has already been taken. Please, use a different one.");

        public static readonly Error NotAllowedToPerformAction =
            Error.Unauthorized(
               code: "User.NotAllowedToPerformAction",
               description: "You are not allowed to perform this action.");

        public static readonly Error RemovedAccountIsNotAdminAccount =
            Error.Validation(
               code: "User.RemovedAccountIsNotAdminAccount",
               description: "Unable to remove the admin account, because the account is not an admin's one.");

        public static readonly Error AccountCannotBeDowngraded =
            Error.Validation(
               code: "User.AccountCannotBeDowngraded",
               description: $"The account cannot be downgraded further as it is already at the lowest possible level.");

        public static readonly Error InvalidCredentials =
            Error.Validation(
                code: "User.InvalidCredentials",
                description: "The provided credentials are invalid.");

        public static readonly Error CannotLogin =
            Error.OperationFailure(
                code: "User.CannotLogin",
                description: "The user cannot login due to some reasons. Check if the email is confirmed.");

        public static readonly Error InvalidAccessToken =
           Error.Validation(
               code: "User.InvalidAccessToken",
               description: "The provided access token is invalid.");

        public static readonly Error InvalidRefreshToken =
            Error.Validation(
                code: "User.InvalidRefreshToken",
                description: "The provided refresh token is invalid.");

        public static readonly Error RefreshTokenExpired =
            Error.Validation(
                code: "User.RefreshTokenExpired",
                description: "The provided refresh token has expired.");

        public static readonly Error CommentAlreadyAdded =
            Error.Validation(
                code: "User.CommentAlreadyAdded",
                description: "The user already has the same comment.");

        public static readonly Error QuestionNotFound =
            Error.Validation(
                code: "User.QuestionNotFound",
                description: "The question has not been found.");

        public static readonly Error CommentNotFound =
            Error.Validation(
               code: "User.CommentNotFound",
               description: "The comment has not been found.");

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

        public static readonly Error CommentAlreadyLikedByUser =
            Error.Validation(
               code: "User.CommentAlreadyLikedByUser",
               description: "The comment is already liked by the user.");

        public static readonly Error CommentAlreadyDislikedByUser =
            Error.Validation(
                code: "User.CommentAlreadyDislikedByUser",
                description: "The comment is already disliked by the user.");

        public static readonly Error CommentNotLiked =
            Error.Validation(
                code: "User.CommentNotLiked",
                description: "The comment is not liked by the user.");

        public static readonly Error CommentNotDisliked =
            Error.Validation(
                code: "User.CommentNotDisliked",
                description: "The comment is not disliked by the user.");

        public static readonly Error CantDislikeCommentIfLiked =
            Error.Validation(
                code: "User.CantDislikeCommentIfLiked",
                description: "Unable to dislike the comment, because the comment has already been liked.");

        public static readonly Error CantLikeCommentIfDisliked =
            Error.Validation(
                code: "User.CantLikeCommentIfDisliked",
                description: "Unable to like the comment, because the comment has already been disliked.");

        public static readonly Error NotAllowedToDowngradeAccount =
            Error.Validation(
                code: "User.NotAllowedToDowngradeAccount",
                description: "You are not allowed to downgrade the account.");

        public static readonly Error EmailAlreadyConfirmed =
            Error.Validation(
                code: "User.EmailAlreadyConfirmed",
                description: "Unable to confirm email, because it's already confirmed.");

        public static readonly Error EmailNotConfirmed =
            Error.Validation(
                code: "User.EmailNotConfirmed",
                description: "Confirm your email to log in to your account.");

        public static readonly Error EmailConfirmationFailure =
            Error.Validation(
                code: "User.EmailConfirmationFailure",
                description: "Failed to confirm the email. The reason may be the token is invalid or expired.");

        public static readonly Error InvalidChangeEmailToken =
           Error.Validation(
               code: "User.InvalidChangeEmailToken",
               description: "Failed to change the email, because the token is invalid.");

        public static readonly Error InvalidGoogleIdToken =
            Error.Validation(
                code: "User.InvalidGoogleIdToken",
                description: "The provided ID token is invalid.");

        public static readonly Error FirstTimeUserSignInPasswordEmpty =
            Error.Validation(
                code: "User.FirstTimeUserSignInPasswordEmpty",
                description: "The first time sign in password of the new user is not provided.");

        public static readonly Error InvalidResetPasswordToken =
            Error.Validation(
                code: "User.InvalidResetPasswordToken",
                description: "Failed to reset the password, because the token is invalid.");

        public static readonly Error CantChangeToSamePlan =
            Error.Validation(
                code: "User.CantChangeToSamePlan",
                description: "Failed to change the plan, because the user already has the same plan.");
    }
}
