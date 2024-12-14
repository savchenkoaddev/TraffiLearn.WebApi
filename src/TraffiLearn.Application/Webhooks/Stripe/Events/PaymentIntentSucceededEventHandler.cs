using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.Domain.Users;

namespace TraffiLearn.Application.Webhooks.Stripe.Events
{
    internal sealed class PaymentIntentSucceededEventHandler
        : INotificationHandler<PaymentIntentSucceededEvent>
    {
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PaymentIntentSucceededEventHandler> _logger;

        public PaymentIntentSucceededEventHandler(
            ISubscriptionPlanRepository subscriptionPlanRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<PaymentIntentSucceededEventHandler> logger)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(
            PaymentIntentSucceededEvent notification,
            CancellationToken cancellationToken)
        {
            var subscriptionPlanId = new SubscriptionPlanId(
                notification.SubscriptionPlanId);
            var userId = new UserId(notification.UserId);

            var subscriptionPlan = await _subscriptionPlanRepository.GetByIdAsync(
                subscriptionPlanId, cancellationToken);

            if (subscriptionPlan is null)
            {
                throw new InvalidOperationException(
                    $"Subscription plan with ID {subscriptionPlanId.Value} not found.");
            }

            var user = await _userRepository.GetByIdAsync(
                userId, cancellationToken);

            if (user is null)
            {
                throw new InvalidOperationException(
                    $"User with ID {userId.Value} not found.");
            }

            var changeResult = user.ChangeSubscriptionPlan(subscriptionPlan);

            if (changeResult.IsFailure)
            {
                throw new InvalidOperationException(
                    $"Failed to change subscription plan for user with ID {userId.Value}. Error description: {changeResult.Error.Description}");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Subscription plan for user {userId} successfully updated to {subscriptionPlanId}.", userId.Value, subscriptionPlanId.Value);
        }
    }
}
