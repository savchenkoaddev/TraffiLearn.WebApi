using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.ConfirmEmail
{
    public sealed record ConfirmEmailCommand(
        Guid? UserId,
        string? Token) : IRequest<Result>;
}
