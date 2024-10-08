﻿namespace TraffiLearn.IntegrationTests.Tickets
{
    internal static class TicketEndpointRoutes
    {
        public static readonly string CreateTicketRoute =
            "api/tickets";

        public static readonly string GetAllTicketsRoute =
            "api/tickets";

        public static readonly string UpdateTicketRoute =
            "api/tickets";

        public static readonly string GetRandomTicketWithQuestionsRoute =
            "api/tickets/random/with-questions";

        public static string GetTicketByIdRoute(Guid ticketId) =>
            $"api/tickets/{ticketId}";

        public static string DeleteTicketRoute(Guid ticketId) =>
            $"api/tickets/{ticketId}";

        public static string AddQuestionToTicketRoute(
            Guid questionId,
            Guid ticketId) => $"api/tickets/{ticketId}/add-question/{questionId}";

        public static string RemoveQuestionFromTicketRoute(
            Guid questionId,
            Guid ticketId) => $"api/tickets/{ticketId}/remove-question/{questionId}";

        public static string GetTicketQuestionsRoute(Guid ticketId) =>
            $"api/tickets/{ticketId}/questions";
    }
}
