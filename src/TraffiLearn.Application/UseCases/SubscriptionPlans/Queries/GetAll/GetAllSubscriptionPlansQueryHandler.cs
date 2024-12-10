using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.SubscriptionPlans.DTO;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.SubscriptionPlans.Queries.GetAll
{
    internal sealed class GetAllSubscriptionPlansQueryHandler
        : IRequestHandler<GetAllSubscriptionPlansQuery, Result<IEnumerable<SubscriptionPlanResponse>>>
    {
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly Mapper<SubscriptionPlan, SubscriptionPlanResponse> _subscriptionPlanMapper;

        public GetAllSubscriptionPlansQueryHandler(
            ISubscriptionPlanRepository subscriptionPlanRepository,
            Mapper<SubscriptionPlan, SubscriptionPlanResponse> subscriptionPlanMapper)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _subscriptionPlanMapper = subscriptionPlanMapper;
        }

        public async Task<Result<IEnumerable<SubscriptionPlanResponse>>> Handle(
            GetAllSubscriptionPlansQuery request,
            CancellationToken cancellationToken)
        {
            var subscriptionPlans = await _subscriptionPlanRepository.GetAllAsync(
                cancellationToken: cancellationToken);

            return Result.Success(_subscriptionPlanMapper.Map(subscriptionPlans));
        }
    }
}
