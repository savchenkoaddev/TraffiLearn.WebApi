using MediatR;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetMarkedQuestions
{
    public sealed record GetMarkedQuestionsQuery
        : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
