using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Commands.RemoveQuestionDislike
{
    public sealed record RemoveQuestionDislikeCommand(
        Guid? QuestionId) : IRequest<Result>;
}
