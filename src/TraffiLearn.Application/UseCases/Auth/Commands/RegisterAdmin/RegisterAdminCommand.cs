using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Auth.Commands.RegisterAdmin
{
    public sealed record RegisterAdminCommand(
        string? Username,
        string? Email,
        string? Password) : IRequest<Result>;
}
