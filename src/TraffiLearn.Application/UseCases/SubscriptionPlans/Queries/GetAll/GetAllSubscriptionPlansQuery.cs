using MediatR;
using TraffiLearn.Application.UseCases.SubscriptionPlans.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.SubscriptionPlans.Queries.GetAll
{
    public sealed record GetAllSubscriptionPlansQuery()
        : IRequest<Result<IEnumerable<SubscriptionPlanResponse>>>;
}
