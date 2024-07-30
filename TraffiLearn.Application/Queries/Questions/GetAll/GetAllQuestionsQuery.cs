using MediatR;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Questions.GetAll
{
    public sealed record GetAllQuestionsQuery : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
