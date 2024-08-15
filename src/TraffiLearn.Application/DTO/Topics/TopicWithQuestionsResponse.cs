using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.DTO.Topics
{
    public sealed record TopicWithQuestionsResponse(
        Guid TopicId,
        int TopicNumber,
        string Title,
        IEnumerable<Question> Questions);
}
