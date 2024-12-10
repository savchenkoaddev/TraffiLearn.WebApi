using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Comments.Commands.DeleteComment
{
    public sealed record DeleteCommentCommand(
        Guid? CommentId) : IRequest<Result>;
}
