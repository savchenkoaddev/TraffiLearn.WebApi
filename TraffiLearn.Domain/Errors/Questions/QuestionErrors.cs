using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Errors.Questions
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
            Error.OperationFailure(
                code: "Question.TopicAlreadyAdded",
                description: "The question already contains the topic.");

        public static readonly Error AnswerAlreadyAdded =
            Error.OperationFailure(
                code: "Question.AnswerAlreadyAdded",
                description: "The question already contains the answer.");

        public static readonly Error AnswerNotFound =
            Error.NotFound(
                code: "Question.AnswerNotFound",
                description: "The question does not contain the answer.");

        public static readonly Error FirstlyAddedAnswerIncorrect =
            Error.OperationFailure(
                code: "Question.FirstlyAddedAnswerIncorrect",
                description: "Newly added answer cannot be incorrect if there are no existing answers in a question.");

        public static readonly Error UnableToRemoveSingleCorrectAnswer =
            Error.OperationFailure(
                code: "Question.UnableToRemoveSingleCorrectAnswer",
                description: "Unable to delete a single correct answer in the question.");

        public static readonly Error NoAnswers =
            Error.OperationFailure(
                code: "Question.NoAnswers",
                description: "There are no answers in the question.");

        public static readonly Error AllAnswersAreIncorrect =
            Error.OperationFailure(
                code: "Question.AllAnswersAreIncorrect",
                description: "There must be at least one correct answer.");

        public static readonly Error DuplicateAnswers =
            Error.OperationFailure(
                code: "Question.DuplicateAnswers",
                description: "There are duplicate answers in the question.");

        public static readonly Error DuplicateTickets =
            Error.OperationFailure(
                code: "Question.DuplicateTickets",
                description: "There are duplicate tickets in the question.");

        public static readonly Error TicketAlreadyAdded =
            Error.OperationFailure(
                code: "Question.TicketAlreadyAdded",
                description: "The question already contains the ticket.");

        public static readonly Error TicketNotFound =
            Error.NotFound(
                code: "Question.TicketNotFound",
                description: "The question does not contain the ticket.");

        public static readonly Error NotEnoughRecords =
            Error.OperationFailure(
                code: "Question.NotEnoughRecords",
                description: "Cannot perform the operation because there are not enough existing records.");
    }
}
