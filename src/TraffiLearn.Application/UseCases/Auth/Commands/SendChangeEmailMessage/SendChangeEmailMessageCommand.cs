using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Auth.Commands.SendChangeEmailMessage
{
    public sealed record SendChangeEmailMessageCommand(
        string? NewEmail) : IRequest<Result>;
}
