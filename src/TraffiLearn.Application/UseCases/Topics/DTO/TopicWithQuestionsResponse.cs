using TraffiLearn.Application.UseCases.Questions.DTO;

namespace TraffiLearn.Application.UseCases.Topics.DTO
{
    public sealed record TopicWithQuestionsResponse(
        Guid TopicId,
        int TopicNumber,
        string Title,
        string? ImageUri,
        IEnumerable<QuestionResponse> Questions);
}
