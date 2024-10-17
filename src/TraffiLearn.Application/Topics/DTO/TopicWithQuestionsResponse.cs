using TraffiLearn.Application.Questions.DTO;

namespace TraffiLearn.Application.Topics.DTO
{
    public sealed record TopicWithQuestionsResponse(
        Guid TopicId,
        int TopicNumber,
        string Title,
        string? ImageUri,
        IEnumerable<QuestionResponse> Questions);
}
