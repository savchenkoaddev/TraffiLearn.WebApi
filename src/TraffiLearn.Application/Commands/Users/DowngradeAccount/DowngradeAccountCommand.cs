using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Users.DowngradeAccount
{
    public sealed record DowngradeAccountCommand(
        Guid? UserId) : IRequest<Result>;
}
