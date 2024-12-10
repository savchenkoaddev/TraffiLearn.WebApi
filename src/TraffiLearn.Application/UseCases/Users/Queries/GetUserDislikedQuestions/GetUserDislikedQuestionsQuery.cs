using MediatR;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetUserDislikedQuestions
{
    public sealed record GetUserDislikedQuestionsQuery(
        Guid? UserId) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
