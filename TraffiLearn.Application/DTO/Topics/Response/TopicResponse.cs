namespace TraffiLearn.Application.DTO.Topics.Response
{
    public sealed record TopicResponse(
        Guid Id,
        int Number,
        string Title);
}
