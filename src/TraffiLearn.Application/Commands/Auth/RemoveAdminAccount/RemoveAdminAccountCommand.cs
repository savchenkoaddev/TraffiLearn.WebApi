using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Auth.RemoveAdminAccount
{
    public sealed record RemoveAdminAccountCommand(
        Guid? AdminId) : IRequest<Result>;
}
