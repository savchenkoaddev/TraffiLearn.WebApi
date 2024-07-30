using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Errors.Tickets
{
    public static class TicketErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "Ticket.NotFound",
                description: "The ticket has not been found.");

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
