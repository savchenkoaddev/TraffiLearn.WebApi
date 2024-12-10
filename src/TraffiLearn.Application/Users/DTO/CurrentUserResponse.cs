using TraffiLearn.Application.SubscriptionPlans.DTO;

namespace TraffiLearn.Application.Users.DTO
{
    public sealed record CurrentUserResponse(
        string Email,
        string Username,
        string Role,
        bool IsEmailConfirmed,
        SubscriptionPlanResponse? SubscriptionPlan,
        DateTime? PlanExpiresOn);
}
