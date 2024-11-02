using MediatR;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Queries.GetCurrentUserDislikedQuestions
{
    public sealed record GetCurrentUserDislikedQuestionsQuery
        : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
