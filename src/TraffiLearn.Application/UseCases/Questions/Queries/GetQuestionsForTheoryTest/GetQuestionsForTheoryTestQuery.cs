using MediatR;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetQuestionsForTheoryTest
{
    public sealed record GetQuestionsForTheoryTestQuery : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
