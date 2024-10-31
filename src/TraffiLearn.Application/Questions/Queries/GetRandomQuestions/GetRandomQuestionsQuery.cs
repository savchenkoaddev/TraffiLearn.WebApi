using MediatR;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Questions.Queries.GetRandomQuestions
{
    public sealed record GetRandomQuestionsQuery(
        int? Amount) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
