using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.RemoveAdminAccount
{
    public sealed record RemoveAdminAccountCommand(
        Guid? AdminId) : IRequest<Result>;
}
