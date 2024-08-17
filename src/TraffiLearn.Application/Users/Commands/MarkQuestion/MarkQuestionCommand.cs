using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Commands.MarkQuestion
{
    public sealed record MarkQuestionCommand(
        Guid? QuestionId) : IRequest<Result>;
}
