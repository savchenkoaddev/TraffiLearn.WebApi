using MediatR;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Queries.GetAll
{
    public sealed record GetAllQuestionsQuery : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
