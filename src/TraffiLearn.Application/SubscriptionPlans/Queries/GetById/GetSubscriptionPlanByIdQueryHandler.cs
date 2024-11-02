using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.SubscriptionPlans.DTO;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.SubscriptionPlans.Queries.GetById
{
    internal sealed class GetSubscriptionPlanByIdQueryHandler
        : IRequestHandler<GetSubscriptionPlanByIdQuery, Result<SubscriptionPlanResponse>>
    {
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly Mapper<SubscriptionPlan, SubscriptionPlanResponse> _subscriptionPlanMapper;

        public GetSubscriptionPlanByIdQueryHandler(
            ISubscriptionPlanRepository subscriptionPlanRepository, 
            Mapper<SubscriptionPlan, SubscriptionPlanResponse> subscriptionPlanMapper)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _subscriptionPlanMapper = subscriptionPlanMapper;
        }

        public async Task<Result<SubscriptionPlanResponse>> Handle(
            GetSubscriptionPlanByIdQuery request, 
            CancellationToken cancellationToken)
        {
            var id = new SubscriptionPlanId(request.SubscriptionPlanId.Value);

            var subscriptionPlan = await _subscriptionPlanRepository.GetByIdAsync(
                id, cancellationToken);

            if (subscriptionPlan is null)
            {
                return Result.Failure<SubscriptionPlanResponse>(
                    SubscriptionPlanErrors.NotFound);
            }

            return Result.Success(_subscriptionPlanMapper.Map(subscriptionPlan));
        }
    }
}
