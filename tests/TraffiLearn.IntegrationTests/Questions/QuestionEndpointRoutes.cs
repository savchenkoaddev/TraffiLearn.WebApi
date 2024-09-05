namespace TraffiLearn.IntegrationTests.Questions
{
    internal static class QuestionEndpointRoutes
    {
        public const string CreateQuestionRoute = "api/questions";

        public const string GetAllQuestionsRoute = "api/questions";

        public const string UpdateQuestionRoute = "api/questions";

        public static string DeleteQuestionRoute(Guid questionId) =>
            $"api/questions/{questionId}";

        public static string GetRandomQuestionsRoute() =>
           $"api/questions/random";

        public static string GetRandomQuestionsRoute(int? amount) =>
            $"api/questions/random?amount={amount}";

        public static string GetQuestionByIdRoute(Guid questionId) =>
            $"api/questions/{questionId}";

        public static string GetQuestionTopicsRoute(Guid questionId) =>
            $"api/questions/{questionId}/topics";

        public static string GetQuestionTicketsRoute(Guid questionId) =>
           $"api/questions/{questionId}/tickets";
    }
}
