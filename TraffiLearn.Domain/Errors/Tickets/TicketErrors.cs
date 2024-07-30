using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Errors.Tickets
{
    public static class TicketErrors
    {
        public static readonly Error NoQuestions =
            Error.Validation(
                code: "Ticket.NoQuestions",
                description: "There are no questions in the ticket.");

        public static readonly Error DuplicateQuestions =
            Error.Validation(
                code: "Ticket.DuplicateQuestions",
                description: "There are duplicate questions in the ticket.");

        public static readonly Error QuestionAlreadyAdded =
            Error.OperationFailure(
                code: "Ticket.QuestionAlreadyAdded",
                description: "The ticket already contains the provided question.");

        public static readonly Error QuestionNotFound =
           Error.NotFound(
               code: "Ticket.QuestionNotFound",
               description: "The question is not found.");
    }
}
