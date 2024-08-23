namespace TraffiLearn.Application.Topics.DTO
{
    public sealed record TopicResponse(
        Guid Id,
        int TopicNumber,
        string Title);
}
