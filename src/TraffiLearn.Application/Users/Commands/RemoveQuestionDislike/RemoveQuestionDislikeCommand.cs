using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Commands.RemoveQuestionDislike
{
    public sealed record RemoveQuestionDislikeCommand(
        Guid? QuestionId) : IRequest<Result>;
}
