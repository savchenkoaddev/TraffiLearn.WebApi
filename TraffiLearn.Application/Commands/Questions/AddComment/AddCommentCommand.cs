using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Questions.AddComment
{
    public sealed record AddCommentCommand(
        Guid? QuestionId,
        string? Content) : IRequest<Result>;
}
