using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Shared.CanceledSubscriptions;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.Domain.Users;
using TraffiLearn.Domain.Users.CancelationReasons;
using TraffiLearn.Domain.Users.DomainEvents;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Mappers
{
    internal sealed class SubscriptionCanceledDomainEventToEntityMapper
        : Mapper<SubscriptionCanceledDomainEvent, Result<CanceledSubscription>>
    {
        public override Result<CanceledSubscription> Map(
            SubscriptionCanceledDomainEvent source)
        {
            CancelationReason? cancelationReason = null;

            if (source.Reason is not null)
            {
                var cancelationReasonResult = CancelationReason.Create(source.Reason);

                if (cancelationReasonResult.IsFailure)
                {
                    return Result.Failure<CanceledSubscription>(
                        cancelationReasonResult.Error);
                }

                cancelationReason = cancelationReasonResult.Value;
            }

            var canceledSubscriptionId = new CanceledSubscriptionId(Guid.NewGuid());
            var subscriptionPlanId = new SubscriptionPlanId(source.SubscriptionId);
            var userId = new UserId(source.UserId);

            var canceledSubscriptionResult = CanceledSubscription.Create(
                canceledSubscriptionId: canceledSubscriptionId,
                userId: userId,
                subscriptionPlanId: subscriptionPlanId,
                cancelationReason: cancelationReason,
                canceledAt: source.CanceledAt);

            if (canceledSubscriptionResult.IsFailure)
            {
                return Result.Failure<CanceledSubscription>(
                    canceledSubscriptionResult.Error);
            }

            return canceledSubscriptionResult.Value;
        }
    }
}
