using MediatR;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Questions.GetQuestionsForTheoryTest
{
    public sealed record GetQuestionsForTheoryTestQuery : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
