using MediatR;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetRandomQuestions
{
    public sealed record GetRandomQuestionsQuery(
        int Amount) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
