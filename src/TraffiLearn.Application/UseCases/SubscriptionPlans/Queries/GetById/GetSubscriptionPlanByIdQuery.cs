using MediatR;
using TraffiLearn.Application.UseCases.SubscriptionPlans.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.SubscriptionPlans.Queries.GetById
{
    public sealed record GetSubscriptionPlanByIdQuery(
        Guid SubscriptionPlanId) : IRequest<Result<SubscriptionPlanResponse>>;
}
