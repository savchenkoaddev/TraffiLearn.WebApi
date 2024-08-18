namespace TraffiLearn.Application.Topics.DTO
{
    public sealed record TopicResponse(
        Guid TopicId,
        int TopicNumber,
        string Title);
}
