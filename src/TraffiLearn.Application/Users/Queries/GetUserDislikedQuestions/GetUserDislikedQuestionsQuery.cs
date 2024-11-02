using MediatR;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Queries.GetUserDislikedQuestions
{
    public sealed record GetUserDislikedQuestionsQuery(
        Guid? UserId) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
