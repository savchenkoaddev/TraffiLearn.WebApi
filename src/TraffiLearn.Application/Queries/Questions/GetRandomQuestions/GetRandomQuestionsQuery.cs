using MediatR;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Questions.GetRandomQuestions
{
    public sealed record GetRandomQuestionsQuery(
        int? Amount) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
