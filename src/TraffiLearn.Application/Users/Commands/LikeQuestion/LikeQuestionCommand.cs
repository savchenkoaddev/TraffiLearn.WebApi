using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Commands.LikeQuestion
{
    public sealed record LikeQuestionCommand(
        Guid? QuestionId) : IRequest<Result>;
}
