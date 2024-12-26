using MediatR;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetCurrentUserLikedQuestions
{
    public sealed record GetCurrentUserLikedQuestionsQuery 
        : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
