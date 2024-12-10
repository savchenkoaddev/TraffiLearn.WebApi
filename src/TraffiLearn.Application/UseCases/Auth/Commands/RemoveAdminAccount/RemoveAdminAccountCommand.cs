using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Auth.Commands.RemoveAdminAccount
{
    public sealed record RemoveAdminAccountCommand(
        Guid? AdminId) : IRequest<Result>;
}
