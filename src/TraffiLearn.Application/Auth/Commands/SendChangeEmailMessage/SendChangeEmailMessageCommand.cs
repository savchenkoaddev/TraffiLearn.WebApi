using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.SendChangeEmailMessage
{
    public sealed record SendChangeEmailMessageCommand(
        string? NewEmail) : IRequest<Result>;
}
