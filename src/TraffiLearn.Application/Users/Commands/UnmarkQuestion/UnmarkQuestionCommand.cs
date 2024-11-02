using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Commands.UnmarkQuestion
{
    public sealed record UnmarkQuestionCommand(
        Guid? QuestionId) : IRequest<Result>;
}
