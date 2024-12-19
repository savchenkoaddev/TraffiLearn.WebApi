using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.CanceledSubscriptions.DTO;
using TraffiLearn.Application.UseCases.SubscriptionPlans.DTO;
using TraffiLearn.Domain.Shared.CanceledSubscriptions;
using TraffiLearn.Domain.SubscriptionPlans;

namespace TraffiLearn.Application.UseCases.CanceledSubscriptions.Mappers
{
    internal sealed class CanceledSubscriptionToResponseMapper
        : Mapper<CanceledSubscription, CanceledSubscriptionResponse>
    {
        private readonly Mapper<SubscriptionPlan, SubscriptionPlanResponse> _subscriptionMapper;

        public CanceledSubscriptionToResponseMapper(
            Mapper<SubscriptionPlan, SubscriptionPlanResponse> subscriptionMapper)
        {
            _subscriptionMapper = subscriptionMapper;
        }

        public override CanceledSubscriptionResponse Map(CanceledSubscription source)
        {
            var subscriptionPlanResponse = _subscriptionMapper.Map(source.SubscriptionPlan);

            return new CanceledSubscriptionResponse(
                Id: source.Id.Value,
                UserId: source.UserId.Value,
                SubscriptionPlan: subscriptionPlanResponse,
                CancelationReason: source.CancelationReason?.Value,
                CanceledAt: source.CanceledAt);
        }
    }
}
