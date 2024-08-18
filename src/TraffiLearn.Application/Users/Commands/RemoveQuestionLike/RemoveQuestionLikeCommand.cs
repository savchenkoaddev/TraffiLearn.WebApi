using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Commands.RemoveQuestionLike
{
    public sealed record RemoveQuestionLikeCommand(
        Guid? QuestionId) : IRequest<Result>;
}
