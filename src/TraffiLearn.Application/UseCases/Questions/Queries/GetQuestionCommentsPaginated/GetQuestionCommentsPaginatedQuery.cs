using MediatR;
using TraffiLearn.Application.UseCases.Comments.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetQuestionCommentsPaginated
{
    public sealed record GetQuestionCommentsPaginatedQuery(
        Guid QuestionId,
        int Page = 1,
        int PageSize = 10) : IRequest<Result<PaginatedCommentsResponse>>;
}
