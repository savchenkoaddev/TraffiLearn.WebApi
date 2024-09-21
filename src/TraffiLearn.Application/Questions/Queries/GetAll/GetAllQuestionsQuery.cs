using MediatR;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Queries.GetAll
{
    public sealed record GetAllQuestionsQuery(
        int Page = 1,
        int PageSize = 10) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
