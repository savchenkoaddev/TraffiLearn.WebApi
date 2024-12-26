using MediatR;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetUserLikedQuestions
{
    public sealed record GetUserLikedQuestionsQuery(
        Guid UserId) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
