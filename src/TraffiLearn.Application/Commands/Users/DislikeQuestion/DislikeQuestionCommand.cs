using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Users.DislikeQuestion
{
    public sealed record DislikeQuestionCommand(
        Guid? QuestionId) : IRequest<Result>;
}
