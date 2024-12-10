namespace TraffiLearn.Application.UseCases.Topics.DTO
{
    public sealed record TopicResponse(
        Guid Id,
        int TopicNumber,
        string Title,
        string? ImageUri);
}
