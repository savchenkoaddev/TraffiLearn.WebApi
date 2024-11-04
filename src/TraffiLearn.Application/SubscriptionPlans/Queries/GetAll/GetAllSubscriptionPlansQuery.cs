using MediatR;
using TraffiLearn.Application.SubscriptionPlans.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.SubscriptionPlans.Queries.GetAll
{
    public sealed record GetAllSubscriptionPlansQuery() 
        : IRequest<Result<IEnumerable<SubscriptionPlanResponse>>>;
}
