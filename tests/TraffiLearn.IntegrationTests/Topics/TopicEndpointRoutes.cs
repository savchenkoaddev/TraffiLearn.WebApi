namespace TraffiLearn.IntegrationTests.Topics
{
    internal static class TopicEndpointRoutes
    {
        public static readonly string CreateTopicRoute =
            "api/topics";

        public static readonly string UpdateTopicRoute =
            "api/topics";

        public static readonly string GetAllSortedTopicsByNumberRoute =
            "api/topics";

        public static string GetTopicQuestionsRoute(Guid topicId) =>
            $"api/topics/{topicId}/questions";

        public static string DeleteTopicRoute(Guid topicId) =>
            $"api/topics/{topicId}";

        public static string AddQuestionToTopicRoute(
            Guid questionId, 
            Guid topicId) => $"api/topics/{topicId}/add-question/{questionId}";

        public static string RemoveQuestionFromTopicRoute(
            Guid questionId,
            Guid topicId) => $"api/topics/{topicId}/remove-question/{questionId}";
    }
}
