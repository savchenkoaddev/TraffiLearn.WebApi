using MediatR;
using TraffiLearn.Application.DTO.Topics.Response;

namespace TraffiLearn.Application.Questions.Queries.GetTopicsForQuestion
{
    public sealed record GetTopicsForQuestionQuery(
        Guid? QuestionId) : IRequest<IEnumerable<TopicResponse>>;
}
