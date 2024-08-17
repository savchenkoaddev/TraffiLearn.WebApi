using MediatR;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Queries.GetCurrentUserDislikedQuestions
{
    public sealed record GetCurrentUserDislikedQuestionsQuery
        : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
