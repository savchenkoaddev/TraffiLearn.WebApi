using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Commands.MarkQuestion
{
    public sealed record MarkQuestionCommand(
        Guid? QuestionId) : IRequest<Result>;
}
