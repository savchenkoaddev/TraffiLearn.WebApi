﻿using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Tickets
{
    public static class TicketErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "Ticket.NotFound",
                description: "The ticket has not been found.");

        public static readonly Error QuestionAlreadyAdded =
            Error.Validation(
                code: "Ticket.QuestionAlreadyAdded",
                description: "The ticket already contains the provided question.");

        public static readonly Error QuestionNotFound =
           Error.NotFound(
               code: "Ticket.QuestionNotFound",
               description: "The question is not found.");
    }
}
