using MediatR;
using TraffiLearn.Application.Auth.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand(
        string? RefreshToken) : IRequest<Result<RefreshTokenResponse>>;
}
