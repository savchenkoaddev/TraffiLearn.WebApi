namespace TraffiLearn.IntegrationTests.Questions
{
    internal static class QuestionEndpointRoutes
    {
        public const string CreateQuestionRoute = "api/questions";

        public const string GetAllQuestionsRoute = "api/questions";

        public static string GetQuestionTopicsRoute(Guid questionId) =>
            $"api/questions/{questionId}/topics";

        public static string GetQuestionTicketsRoute(Guid questionId) =>
           $"api/questions/{questionId}/tickets";
    }
}
