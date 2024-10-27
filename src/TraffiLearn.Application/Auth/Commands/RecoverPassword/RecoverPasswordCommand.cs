using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.RecoverPassword
{
    public sealed record RecoverPasswordCommand(
        string? Email) : IRequest<Result>;
}
