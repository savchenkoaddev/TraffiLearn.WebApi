using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.Domain.Users;
using TraffiLearn.Domain.Users.CancelationReasons;
using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Shared.CanceledSubscriptions
{
    public sealed class CanceledSubscription : Entity<CanceledSubscriptionId>
    {
        private CanceledSubscription()
            : base(new(Guid.Empty))
        { }

        private CanceledSubscription(
            CanceledSubscriptionId canceledSubscriptionId,
            UserId userId,
            SubscriptionPlanId subscriptionPlanId,
            CancelationReason? cancelationReason,
            DateTime canceledAt) : base(canceledSubscriptionId)
        {
            UserId = userId;
            SubscriptionPlanId = subscriptionPlanId;
            CancelationReason = cancelationReason;
            CanceledAt = canceledAt;
        }

        public UserId UserId { get; private init; }

        public SubscriptionPlanId SubscriptionPlanId { get; private init; }

        public SubscriptionPlan SubscriptionPlan { get; private init; }

        public CancelationReason? CancelationReason { get; private init; } = default;

        public DateTime CanceledAt { get; private init; }

        public static Result<CanceledSubscription> Create(
            CanceledSubscriptionId canceledSubscriptionId,
            UserId userId,
            SubscriptionPlanId subscriptionPlanId,
            CancelationReason? cancelationReason,
            DateTime canceledAt)
        {
            return new CanceledSubscription(
                canceledSubscriptionId,
                userId, 
                subscriptionPlanId, 
                cancelationReason, 
                canceledAt);
        }
    }
}
