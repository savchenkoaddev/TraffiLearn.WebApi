namespace TraffiLearn.Application.DTO.Topics
{
    public sealed record TopicResponse(
        Guid TopicId,
        int TopicNumber,
        string Title);
}
