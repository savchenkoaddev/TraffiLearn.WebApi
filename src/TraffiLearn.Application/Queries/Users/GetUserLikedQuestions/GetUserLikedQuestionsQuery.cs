using MediatR;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Users.GetUserLikedQuestions
{
    public sealed record GetUserLikedQuestionsQuery(
        Guid? UserId) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
