using TraffiLearn.Application.UseCases.SubscriptionPlans.DTO;

namespace TraffiLearn.Application.UseCases.Users.DTO
{
    public sealed record CurrentUserResponse(
        string Email,
        string Username,
        string Role,
        bool IsEmailConfirmed,
        SubscriptionPlanResponse? SubscriptionPlan,
        DateTime? PlanExpiresOn);
}
