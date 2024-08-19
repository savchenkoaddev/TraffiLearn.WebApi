using MediatR;
using TraffiLearn.Application.Comments.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Queries.GetQuestionComments
{
    public sealed record GetQuestionCommentsQuery(
        Guid? QuestionId) : IRequest<Result<IEnumerable<CommentResponse>>>;
}
