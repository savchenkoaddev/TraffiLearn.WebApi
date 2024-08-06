using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Users.RemoveQuestionLike
{
    public sealed record RemoveQuestionLikeCommand(
        Guid? QuestionId) : IRequest<Result>;
}
