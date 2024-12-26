using MediatR;
using TraffiLearn.Application.UseCases.Auth.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Auth.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand(
        string AccessToken,
        string RefreshToken) : IRequest<Result<RefreshTokenResponse>>;
}
