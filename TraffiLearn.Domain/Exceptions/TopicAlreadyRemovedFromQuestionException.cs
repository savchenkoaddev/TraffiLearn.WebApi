namespace TraffiLearn.Domain.Exceptions
{
    public sealed class TopicAlreadyRemovedFromQuestionException : Exception
    {
        public TopicAlreadyRemovedFromQuestionException() : base("The provided question does not contain the provided topic.")
        { }

        public TopicAlreadyRemovedFromQuestionException(Guid topicId, Guid questionId) : base($"The question with '{questionId}' id does not contain the topic with '{topicId}' id.")
        { }
    }
}
