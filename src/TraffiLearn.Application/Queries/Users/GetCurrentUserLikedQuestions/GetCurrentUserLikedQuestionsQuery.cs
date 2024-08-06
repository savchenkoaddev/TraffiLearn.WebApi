using MediatR;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Users.GetCurrentUserLikedQuestions
{
    public sealed record GetCurrentUserLikedQuestionsQuery : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
