using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Commands.DowngradeAccount
{
    public sealed record DowngradeAccountCommand(
        Guid UserId) : IRequest<Result>;
}
