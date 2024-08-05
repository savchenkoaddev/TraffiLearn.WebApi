using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Users.LikeQuestion
{
    public sealed record LikeQuestionCommand(
        Guid? QuestionId) : IRequest<Result>;
}
