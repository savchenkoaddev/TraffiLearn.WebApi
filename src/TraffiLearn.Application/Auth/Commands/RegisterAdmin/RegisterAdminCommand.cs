using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Auth.RegisterAdmin
{
    public sealed record RegisterAdminCommand(
        string? Username,
        string? Email,
        string? Password) : IRequest<Result>;
}
