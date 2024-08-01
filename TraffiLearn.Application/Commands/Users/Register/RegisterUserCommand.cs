using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Users.Register
{
    public sealed record RegisterUserCommand(
        string? Username,
        string? Email,
        string? Password) : IRequest<Result>;
}
