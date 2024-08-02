using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Comments.AddComment
{
    public sealed record AddCommentCommand(
        Guid? QuestionId,
        string? Content) : IRequest<Result>;
}
