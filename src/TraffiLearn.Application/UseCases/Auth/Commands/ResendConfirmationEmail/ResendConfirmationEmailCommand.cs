using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Auth.Commands.ResendConfirmationEmail
{
    public sealed record ResendConfirmationEmailCommand(
        string Email) : IRequest<Result>;
}
