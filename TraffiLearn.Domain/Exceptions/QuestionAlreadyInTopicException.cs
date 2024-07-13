namespace TraffiLearn.Domain.Exceptions
{
    public sealed class QuestionAlreadyInTopicException : Exception
    {
        public QuestionAlreadyInTopicException() : base("The provided topic already contains the provided question.")
        { }

        public QuestionAlreadyInTopicException(Guid topicId, Guid questionId) : base($"The topic with '{topicId}' id already contains the question with '{questionId}' id.")
        { }
    }
}
