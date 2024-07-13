using MediatR;
using TraffiLearn.Application.DTO.Questions.Response;

namespace TraffiLearn.Application.Topics.Queries.GetQuestionsForTopic
{
    public sealed record GetQuestionsForTopicQuery(
        Guid? TopicId) : IRequest<IEnumerable<QuestionResponse>>;
}
