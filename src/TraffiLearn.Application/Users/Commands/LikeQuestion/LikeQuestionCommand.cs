using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Commands.LikeQuestion
{
    public sealed record LikeQuestionCommand(
        Guid? QuestionId) : IRequest<Result>;
}
