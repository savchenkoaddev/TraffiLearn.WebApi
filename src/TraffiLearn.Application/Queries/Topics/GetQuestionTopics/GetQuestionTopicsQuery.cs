using MediatR;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Topics.GetQuestionTopics
{
    public sealed record GetQuestionTopicsQuery(
        Guid? QuestionId) : IRequest<Result<IEnumerable<TopicResponse>>>;
}
