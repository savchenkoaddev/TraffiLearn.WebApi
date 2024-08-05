using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Users.UnmarkQuestion
{
    public sealed record UnmarkQuestionCommand(
        Guid? QuestionId) : IRequest<Result>;
}
