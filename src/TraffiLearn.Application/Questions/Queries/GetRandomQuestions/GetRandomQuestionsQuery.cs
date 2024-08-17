using MediatR;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Queries.GetRandomQuestions
{
    public sealed record GetRandomQuestionsQuery(
        int? Amount) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
