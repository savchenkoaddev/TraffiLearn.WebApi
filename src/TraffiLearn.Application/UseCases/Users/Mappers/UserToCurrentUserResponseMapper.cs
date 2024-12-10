using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.SubscriptionPlans.DTO;
using TraffiLearn.Application.UseCases.Users.DTO;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.Domain.Users;

namespace TraffiLearn.Application.UseCases.Users.Mappers
{
    internal sealed class UserToCurrentUserResponseMapper
        : Mapper<User, CurrentUserResponse>
    {
        private readonly Mapper<SubscriptionPlan,
            SubscriptionPlanResponse> _subscriptionPlanMapper;

        public UserToCurrentUserResponseMapper(
            Mapper<SubscriptionPlan, SubscriptionPlanResponse> subscriptionPlanMapper)
        {
            _subscriptionPlanMapper = subscriptionPlanMapper;
        }

        public override CurrentUserResponse Map(User source)
        {
            SubscriptionPlanResponse? subscriptionPlanResponse = default!;
            DateTime? planExpiresOn = default!;

            if (source.SubscriptionPlan is not null)
            {
                subscriptionPlanResponse = _subscriptionPlanMapper
                    .Map(source.SubscriptionPlan);

                planExpiresOn = source.PlanExpiresOn;
            }

            return new CurrentUserResponse(
                Email: source.Email.Value,
                Username: source.Username.Value,
                Role: source.Role.ToString(),
                IsEmailConfirmed: source.IsEmailConfirmed,
                SubscriptionPlan: subscriptionPlanResponse,
                PlanExpiresOn: planExpiresOn);
        }
    }
}
