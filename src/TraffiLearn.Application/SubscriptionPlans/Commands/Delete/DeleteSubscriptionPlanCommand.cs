using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.SubscriptionPlans.Commands.Delete
{
    public sealed record DeleteSubscriptionPlanCommand(
        Guid? SubscriptionPlanId) : IRequest<Result>;
}
