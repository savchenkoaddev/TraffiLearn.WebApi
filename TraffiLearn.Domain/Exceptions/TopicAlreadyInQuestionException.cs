namespace TraffiLearn.Domain.Exceptions
{
    public sealed class TopicAlreadyInQuestionException : Exception
    {
        public TopicAlreadyInQuestionException() : base("The provided question already contains the provided topic.")
        { }

        public TopicAlreadyInQuestionException(Guid topicId, Guid questionId) : base($"The question with '{questionId}' id already contains the topic with '{topicId}' id.")
        { }
    }
}
