using MediatR;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Application.Queries.Questions.GetTopicsForQuestion
{
    public sealed record GetTopicsForQuestionQuery(
        Guid? QuestionId) : IRequest<Result<IEnumerable<TopicResponse>>>;
}
