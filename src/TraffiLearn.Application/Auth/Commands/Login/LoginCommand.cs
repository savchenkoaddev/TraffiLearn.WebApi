using MediatR;
using TraffiLearn.Application.Auth.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.Login
{
    public sealed record LoginCommand(
        string? Email,
        string? Password) : IRequest<Result<LoginResponse>>;
}
