using MediatR;
using TraffiLearn.Application.DTO.Auth;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.Login
{
    public sealed record LoginCommand(
        string? Email,
        string? Password) : IRequest<Result<LoginResponse>>;
}
