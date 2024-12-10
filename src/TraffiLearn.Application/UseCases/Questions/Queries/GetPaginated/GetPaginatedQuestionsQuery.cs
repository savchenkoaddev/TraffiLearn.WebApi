using MediatR;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetPaginated
{
    public sealed record GetPaginatedQuestionsQuery(
        int Page = 1,
        int PageSize = 10) : IRequest<Result<PaginatedQuestionsResponse>>;
}
