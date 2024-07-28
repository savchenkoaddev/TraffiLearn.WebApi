using MediatR;
using TraffiLearn.Application.DTO.Topics;

namespace TraffiLearn.Application.Queries.Questions.GetTopicsForQuestion
{
    public sealed record GetTopicsForQuestionQuery(
        Guid? QuestionId) : IRequest<IEnumerable<TopicResponse>>;
}
