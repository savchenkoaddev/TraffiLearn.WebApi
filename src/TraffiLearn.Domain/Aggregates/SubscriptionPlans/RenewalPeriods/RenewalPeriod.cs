using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.SubscriptionPlans.RenewalPeriods
{
    public sealed class RenewalPeriod : ValueObject
    {
        private RenewalPeriod(
            int interval,
            RenewalPeriodType type)
        {
            Interval = interval;
            Type = type;
        }

        public int Interval { get; }

        public RenewalPeriodType Type { get; }

        public static Result<RenewalPeriod> Create(
            int interval,
            RenewalPeriodType type)
        {
            if (interval < 0)
            {
                return Result.Failure<RenewalPeriod>(
                    RenewalPeriodErrors.NegativeInterval);
            }

            return new RenewalPeriod(
                interval, type);
        }

        public int GetDaysEquivalent()
        {
            return Type switch
            {
                RenewalPeriodType.Days => Interval,
                RenewalPeriodType.Weeks => Interval * 7,
                RenewalPeriodType.Months => Interval * 30,
                RenewalPeriodType.Years => Interval * 365,
                _ => throw new InvalidOperationException(
                    "Unsupported RenewalPeriodType.")
            };
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Interval;
            yield return Type;
        }
    }
}
