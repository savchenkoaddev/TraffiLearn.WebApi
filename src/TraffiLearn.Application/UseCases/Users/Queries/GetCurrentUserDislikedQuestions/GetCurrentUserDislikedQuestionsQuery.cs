using MediatR;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetCurrentUserDislikedQuestions
{
    public sealed record GetCurrentUserDislikedQuestionsQuery
        : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
