using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.SubscriptionPlans.DTO;
using TraffiLearn.Application.UseCases.Transactions.DTO;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.Domain.Transactions;

namespace TraffiLearn.Application.UseCases.Transactions.Mappers
{
    internal sealed class TransactionToTransactionResponseMapper
        : Mapper<Transaction, TransactionResponse>
    {
        private readonly Mapper<SubscriptionPlan, SubscriptionPlanResponse> _subscriptionMapper;

        public TransactionToTransactionResponseMapper(
            Mapper<SubscriptionPlan, SubscriptionPlanResponse> subscriptionMapper)
        {
            _subscriptionMapper = subscriptionMapper;
        }

        public override TransactionResponse Map(Transaction source)
        {
            var subscriptionPlanResponse = _subscriptionMapper.Map(source.SubscriptionPlan);

            return new TransactionResponse(
                Id: source.Id.Value,
                SubscriptionPlan: subscriptionPlanResponse,
                Timestamp: source.Timestamp,
                Metadata: source.Metadata?.Value);
        }
    }
}
