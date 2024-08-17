using MediatR;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Topics.Queries.GetQuestionTopics
{
    public sealed record GetQuestionTopicsQuery(
        Guid? QuestionId) : IRequest<Result<IEnumerable<TopicResponse>>>;
}
