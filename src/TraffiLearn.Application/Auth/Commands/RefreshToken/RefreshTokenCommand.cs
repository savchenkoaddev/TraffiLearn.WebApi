using MediatR;
using TraffiLearn.Application.Auth.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Auth.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand(
        string? AccessToken,
        string? RefreshToken) : IRequest<Result<RefreshTokenResponse>>;
}
