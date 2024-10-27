using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.SendRecoverPasswordMessage
{
    public sealed record SendRecoverPasswordMessageCommand(
        string? Email) : IRequest<Result>;
}
