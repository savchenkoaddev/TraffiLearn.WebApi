using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.RecoverPassword
{
    public sealed record RecoverPasswordCommand(
        Guid? UserId,
        string? Token,
        string? NewPassword,
        string? RepeatPassword) : IRequest<Result>;
}
