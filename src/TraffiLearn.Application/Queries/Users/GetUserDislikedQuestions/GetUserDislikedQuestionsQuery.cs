using MediatR;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Users.GetUserDislikedQuestions
{
    public sealed record GetUserDislikedQuestionsQuery(
        Guid? UserId) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
