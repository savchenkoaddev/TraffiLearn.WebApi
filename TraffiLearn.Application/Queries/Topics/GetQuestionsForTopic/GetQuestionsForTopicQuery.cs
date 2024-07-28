using MediatR;
using TraffiLearn.Application.DTO.Questions;

namespace TraffiLearn.Application.Queries.Topics.GetQuestionsForTopic
{
    public sealed record GetQuestionsForTopicQuery(Guid TopicId) : IRequest<IEnumerable<QuestionResponse>>;
}
