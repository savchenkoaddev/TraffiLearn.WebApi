using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Users.RemoveQuestionDislike
{
    public sealed record RemoveQuestionDislikeCommand(
        Guid? QuestionId) : IRequest<Result>;
}
