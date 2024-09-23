using MediatR;
using TraffiLearn.Application.Comments.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Queries.GetQuestionCommentsPaginated
{
    public sealed record GetQuestionCommentsPaginatedQuery(
        Guid? QuestionId,
        int Page = 1,
        int PageCount = 10) : IRequest<Result<IEnumerable<CommentResponse>>>;
}
