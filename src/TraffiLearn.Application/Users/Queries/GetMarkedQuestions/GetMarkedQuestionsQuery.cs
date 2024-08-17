using MediatR;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Queries.GetMarkedQuestions
{
    public sealed record GetMarkedQuestionsQuery
        : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
