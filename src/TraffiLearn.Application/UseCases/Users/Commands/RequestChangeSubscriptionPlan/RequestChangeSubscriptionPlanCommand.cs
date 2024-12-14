using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Commands.RequestChangeSubscriptionPlan
{
    public sealed record RequestChangeSubscriptionPlanCommand(
        Guid? SubscriptionPlanId) : IRequest<Result<Uri>>;
}
