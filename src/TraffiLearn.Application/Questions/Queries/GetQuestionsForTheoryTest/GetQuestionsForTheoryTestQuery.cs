using MediatR;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Queries.GetQuestionsForTheoryTest
{
    public sealed record GetQuestionsForTheoryTestQuery : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
