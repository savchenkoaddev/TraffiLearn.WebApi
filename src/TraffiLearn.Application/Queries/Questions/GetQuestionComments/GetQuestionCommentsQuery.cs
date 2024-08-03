using MediatR;
using TraffiLearn.Application.DTO.Comments;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Questions.GetQuestionComments
{
    public sealed record GetQuestionCommentsQuery(
        Guid? QuestionId) : IRequest<Result<IEnumerable<CommentResponse>>>;
}
