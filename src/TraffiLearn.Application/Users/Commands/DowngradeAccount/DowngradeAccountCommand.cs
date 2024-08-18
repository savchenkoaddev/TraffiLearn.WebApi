using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Commands.DowngradeAccount
{
    public sealed record DowngradeAccountCommand(
        Guid? UserId) : IRequest<Result>;
}
