using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.ResetPassword
{
    public sealed record ResetPasswordCommand(
        Guid? UserId,
        string? Token,
        string? NewPassword,
        string? RepeatPassword) : IRequest<Result>;
}
