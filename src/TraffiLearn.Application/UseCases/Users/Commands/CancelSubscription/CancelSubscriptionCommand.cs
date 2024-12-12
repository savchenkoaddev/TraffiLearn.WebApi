using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Commands.CancelSubscription
{
    public sealed record CancelSubscriptionCommand(
        string? Reason) : IRequest<Result>;
}
