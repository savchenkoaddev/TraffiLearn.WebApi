namespace TraffiLearn.Domain.Exceptions
{
    public sealed class TopicNotFoundException : Exception
    {
        public TopicNotFoundException() : base("Topic with the provided id has not been found.")
        { }

        public TopicNotFoundException(Guid topicId) : base($"Topic with the '{topicId}' id has not been found.")
        { }
    }
}
