using MediatR;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Queries.GetUserLikedQuestions
{
    public sealed record GetUserLikedQuestionsQuery(
        Guid? UserId) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
