using MediatR;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Queries.GetCurrentUserLikedQuestions
{
    public sealed record GetCurrentUserLikedQuestionsQuery : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
