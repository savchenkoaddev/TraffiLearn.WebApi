using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.DTO.Topics
{
    public sealed record TopicResponse(
        Guid Id,
        int TopicNumber,
        string Title);
}
