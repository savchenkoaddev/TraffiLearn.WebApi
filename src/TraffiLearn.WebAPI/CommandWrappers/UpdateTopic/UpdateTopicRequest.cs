namespace TraffiLearn.WebAPI.CommandWrappers.UpdateTopic
{
    public sealed record UpdateTopicRequest(
        Guid? TopicId,
        int? TopicNumber,
        string? Title,
        bool? RemoveOldImageIfNewMissing);
}
