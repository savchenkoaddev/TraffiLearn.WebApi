using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Commands.ChangeSubscriptionPlan
{
    public sealed record ChangeSubscriptionPlanCommand(
        Guid? SubscriptionPlanId) : IRequest<Result>;
}
