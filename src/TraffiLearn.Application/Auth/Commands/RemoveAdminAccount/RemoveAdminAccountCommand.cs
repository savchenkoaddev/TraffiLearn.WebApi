using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Auth.Commands.RemoveAdminAccount
{
    public sealed record RemoveAdminAccountCommand(
        Guid? AdminId) : IRequest<Result>;
}
