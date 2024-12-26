namespace TraffiLearn.WebAPI.CommandWrappers.CreateTopic
{
    public sealed record CreateTopicRequest(
        int TopicNumber,
        string Title);
}
