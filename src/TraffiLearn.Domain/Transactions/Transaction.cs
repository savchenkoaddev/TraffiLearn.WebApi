using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.Domain.Transactions.Metadatas;
using TraffiLearn.Domain.Users;
using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Transactions
{
    public sealed class Transaction : AggregateRoot<TransactionId>
    {
        private Transaction()
            : base(new(Guid.Empty))
        { }

        private Transaction(
            TransactionId id,
            User user,
            SubscriptionPlan subscriptionPlan,
            DateTime timestamp,
            Metadata? metadata) : base(id)
        {
            User = user;
            SubscriptionPlan = subscriptionPlan;
            Timestamp = timestamp;
            Metadata = metadata;
        }

        public User User { get; private init; }

        public SubscriptionPlan SubscriptionPlan { get; private init; }

        public DateTime Timestamp { get; private init; }

        public Metadata? Metadata { get; private init; }

        public static Result<Transaction> Create(
            TransactionId id,
            User user,
            SubscriptionPlan subscriptionPlan,
            DateTime timestamp,
            Metadata? metadata)
        {
            return new Transaction(
                id,
                user,
                subscriptionPlan,
                timestamp,
                metadata);
        }
    }
}
