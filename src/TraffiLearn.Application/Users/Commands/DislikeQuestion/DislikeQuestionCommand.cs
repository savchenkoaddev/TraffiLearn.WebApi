using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Commands.DislikeQuestion
{
    public sealed record DislikeQuestionCommand(
        Guid? QuestionId) : IRequest<Result>;
}
