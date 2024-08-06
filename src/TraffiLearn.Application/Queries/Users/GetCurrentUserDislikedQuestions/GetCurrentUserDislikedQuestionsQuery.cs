using MediatR;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Users.GetCurrentUserDislikedQuestions
{
    public sealed record GetCurrentUserDislikedQuestionsQuery
        : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
