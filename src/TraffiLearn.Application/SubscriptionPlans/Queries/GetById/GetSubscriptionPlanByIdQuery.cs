using MediatR;
using TraffiLearn.Application.SubscriptionPlans.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.SubscriptionPlans.Queries.GetById
{
    public sealed record GetSubscriptionPlanByIdQuery(
        Guid? SubscriptionPlanId) : IRequest<Result<SubscriptionPlanResponse>>;
}
