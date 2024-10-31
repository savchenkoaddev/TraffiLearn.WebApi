using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Auth.Commands.SendChangeEmailMessage
{
    public sealed record SendChangeEmailMessageCommand(
        string? NewEmail) : IRequest<Result>;
}
