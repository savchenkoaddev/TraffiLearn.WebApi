using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Commands.UnmarkQuestion
{
    public sealed record UnmarkQuestionCommand(
        Guid? QuestionId) : IRequest<Result>;
}
