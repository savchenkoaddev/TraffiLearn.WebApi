using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Auth.Commands.SendRecoverPasswordMessage
{
    public sealed record SendRecoverPasswordMessageCommand(
        string? Email) : IRequest<Result>;
}
