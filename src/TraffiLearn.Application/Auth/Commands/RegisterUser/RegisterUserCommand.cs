using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Auth.Commands.RegisterUser
{
    public sealed record RegisterUserCommand(
        string? Username,
        string? Email,
        string? Password) : IRequest<Result>;
}
