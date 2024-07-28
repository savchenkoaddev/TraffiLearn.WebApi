using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.DTO.Topics
{
    public sealed record TopicResponse(
        Guid Id,
        TopicNumber TopicNumber,
        TopicTitle Title);
}
