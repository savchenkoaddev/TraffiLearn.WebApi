using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Commands.ChangeSubscriptionPlan
{
    public sealed record ChangeSubscriptionPlanCommand(
        Guid? SubscriptionPlanId) : IRequest<Result>;
}
