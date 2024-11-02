using MediatR;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Questions.Queries.GetQuestionTopics
{
    public sealed record GetQuestionTopicsQuery(
        Guid? QuestionId) : IRequest<Result<IEnumerable<TopicResponse>>>;
}
