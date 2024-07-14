using MediatR;
using TraffiLearn.Application.DTO.Questions.Response;

namespace TraffiLearn.Application.Questions.Queries.GetAllQuestions
{
    public sealed record GetAllQuestionsQuery : IRequest<IEnumerable<QuestionResponse>>;
}
