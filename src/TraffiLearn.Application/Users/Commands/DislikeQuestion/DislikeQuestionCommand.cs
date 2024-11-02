using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Commands.DislikeQuestion
{
    public sealed record DislikeQuestionCommand(
        Guid? QuestionId) : IRequest<Result>;
}
