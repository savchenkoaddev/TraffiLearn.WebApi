using TraffiLearn.Application.UseCases.SubscriptionPlans.DTO;

namespace TraffiLearn.Application.UseCases.Transactions.DTO
{
    public sealed record TransactionResponse(
        Guid Id,
        Guid UserId,
        SubscriptionPlanResponse SubscriptionPlan,
        DateTime Timestamp,
        string? Metadata);
}
