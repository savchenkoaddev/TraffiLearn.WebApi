using MediatR;
using TraffiLearn.Application.UseCases.Auth.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Auth.Commands.Login
{
    public sealed record LoginCommand(
        string? Email,
        string? Password) : IRequest<Result<LoginResponse>>;
}
