using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Users.MarkQuestion
{
    public sealed record MarkQuestionCommand(
        Guid? QuestionId) : IRequest<Result>;
}
