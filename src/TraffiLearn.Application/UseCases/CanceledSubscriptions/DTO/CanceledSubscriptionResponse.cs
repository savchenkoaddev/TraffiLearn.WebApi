using TraffiLearn.Application.UseCases.SubscriptionPlans.DTO;

namespace TraffiLearn.Application.UseCases.CanceledSubscriptions.DTO
{
    public sealed record CanceledSubscriptionResponse(
        Guid Id,
        Guid UserId,
        SubscriptionPlanResponse SubscriptionPlan,
        DateTime CanceledAt,
        string? CancelationReason);
}
