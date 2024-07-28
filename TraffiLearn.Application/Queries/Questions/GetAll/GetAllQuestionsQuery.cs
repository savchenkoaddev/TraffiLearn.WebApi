using MediatR;
using TraffiLearn.Application.DTO.Questions;

namespace TraffiLearn.Application.Queries.Questions.GetAll
{
    public sealed record GetAllQuestionsQuery : IRequest<IEnumerable<QuestionResponse>>;
}
