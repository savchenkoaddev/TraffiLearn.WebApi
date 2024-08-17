using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Questions.Errors
{
    public static class QuestionErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "Question.NotFound",
                description: "Question has not been found.");

        public static readonly Error TopicNotFound =
            Error.NotFound(
                code: "Question.TopicNotFound",
                description: "The question does not contain the provided topic.");

        public static readonly Error TopicAlreadyAdded =
            Error.Validation(
                code: "Question.TopicAlreadyAdded",
                description: "The question already contains the topic.");

        public static readonly Error AnswerAlreadyAdded =
            Error.Validation(
                code: "Question.AnswerAlreadyAdded",
                description: "The question already contains the answer.");

        public static readonly Error AnswerNotFound =
            Error.NotFound(
                code: "Question.AnswerNotFound",
                description: "The question does not contain the answer.");

        public static readonly Error FirstlyAddedAnswerIncorrect =
            Error.Validation(
                code: "Question.FirstlyAddedAnswerIncorrect",
                description: "Newly added answer cannot be incorrect if there are no existing answers in a question.");

        public static readonly Error UnableToRemoveSingleCorrectAnswer =
            Error.Validation(
                code: "Question.UnableToRemoveSingleCorrectAnswer",
                description: "Unable to delete a single correct answer in the question.");

        public static readonly Error NoAnswers =
            Error.Validation(
                code: "Question.NoAnswers",
                description: "There are no answers in the question.");

        public static readonly Error AllAnswersAreIncorrect =
            Error.Validation(
                code: "Question.AllAnswersAreIncorrect",
                description: "There must be at least one correct answer.");

        public static readonly Error DuplicateAnswers =
            Error.Validation(
                code: "Question.DuplicateAnswers",
                description: "There are duplicate answers in the question.");

        public static readonly Error DuplicateTickets =
            Error.Validation(
                code: "Question.DuplicateTickets",
                description: "There are duplicate tickets in the question.");

        public static readonly Error TicketAlreadyAdded =
            Error.Validation(
                code: "Question.TicketAlreadyAdded",
                description: "The question already contains the ticket.");

        public static readonly Error TicketNotFound =
            Error.NotFound(
                code: "Question.TicketNotFound",
                description: "The question does not contain the ticket.");

        public static readonly Error CommentAlreadyAdded =
            Error.Validation(
                code: "Question.CommentAlreadyAdded",
                description: "The question already contains the comment.");

        public static readonly Error AlreadyLikedByUser =
            Error.Validation(
                code: "Question.AlreadyLikedByUser",
                description: "The question is already liked by the user.");

        public static readonly Error AlreadyDislikedByUser =
            Error.Validation(
                code: "Question.AlreadyDislikedByUser",
                description: "The question is already disliked by the user.");

        public static readonly Error AlreadyMarkedByUser =
            Error.Validation(
                code: "Question.AlreadyMarkedByUser",
                description: "The question is already marked by the user.");

        public static readonly Error IsNotMarkedByUser =
            Error.Validation(
                code: "Question.IsNotMarkedByUser",
                description: "The question is not marked yet.");

        public static readonly Error NotLikedByUser =
            Error.Validation(
                code: "Question.NotLikedByUser",
                description: "The question is not liked by the user.");

        public static readonly Error NotDislikedByUser =
            Error.Validation(
                code: "Question.NotDislikedByUser",
                description: "The question is not disliked by the user.");

        public static readonly Error CantDislikeIfLikedByUser =
            Error.Validation(
                code: "Question.CantDislikeIfLikedByUser",
                description: "Unable to dislike the question, because the question has already been liked by the user.");

        public static readonly Error CantLikeIfDislikedByUser =
            Error.Validation(
                code: "Question.CantLikeIfDislikedByUser",
                description: "Unable to like the question, because the question has already been disliked by the user.");
    }
}
