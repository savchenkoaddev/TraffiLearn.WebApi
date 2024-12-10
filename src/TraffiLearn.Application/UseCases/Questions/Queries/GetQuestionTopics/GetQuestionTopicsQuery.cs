using MediatR;
using TraffiLearn.Application.UseCases.Topics.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetQuestionTopics
{
    public sealed record GetQuestionTopicsQuery(
        Guid? QuestionId) : IRequest<Result<IEnumerable<TopicResponse>>>;
}
