using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Auth.Commands.ConfirmChangeEmail
{
    public sealed record ConfirmChangeEmailCommand(
        Guid UserId,
        string Token,
        string NewEmail) : IRequest<Result>;
}
