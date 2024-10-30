using TraffiLearn.Domain.Aggregates.SubscriptionPlans.ValueObjects;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Aggregates.SubscriptionPlans
{
    public sealed class SubscriptionPlan : AggregateRoot<SubscriptionPlanId>
    {
        private SubscriptionPlan()
            : base(new(Guid.Empty))
        { }
    }
}
