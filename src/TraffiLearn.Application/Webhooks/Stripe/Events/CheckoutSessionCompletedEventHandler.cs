using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.Domain.Transactions;
using TraffiLearn.Domain.Transactions.Metadatas;
using TraffiLearn.Domain.Users;
using TraffiLearn.SharedKernel.ValueObjects.Prices;

namespace TraffiLearn.Application.Webhooks.Stripe.Events
{
    internal sealed class CheckoutSessionCompletedEventHandler
        : INotificationHandler<CheckoutSessionCompletedEvent>
    {
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CheckoutSessionCompletedEventHandler> _logger;

        public CheckoutSessionCompletedEventHandler(
            ISubscriptionPlanRepository subscriptionPlanRepository,
            ITransactionRepository transactionRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<CheckoutSessionCompletedEventHandler> logger)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(
            CheckoutSessionCompletedEvent notification,
            CancellationToken cancellationToken)
        {
            var subscriptionPlan = await GetSubscriptionPlanAsync(
                notification.SubscriptionPlanId,
                cancellationToken);

            var user = await GetUserAsync(
                notification.UserId,
                cancellationToken);

            ChangeUserSubscriptionPlan(user, subscriptionPlan);

            Metadata? metadata = default;

            if (notification.Metadata is not null)
            {
                var metadataResult = Metadata.Create(notification.Metadata);

                if (metadataResult.IsSuccess)
                {
                    metadata = metadataResult.Value;
                }
                else
                {
                    _logger.LogWarning(
                        "Failed to create a metadata object. Error: {error}.", metadataResult.Error);
                }
            }

            var transaction = CreateTransaction(
                user,
                subscriptionPlan,
                metadata);

            await _transactionRepository.InsertAsync(
                transaction,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Subscription plan for user {userId} successfully updated to {subscriptionPlanId}.",
                user.Id.Value, subscriptionPlan.Id.Value);
        }

        private async Task<SubscriptionPlan> GetSubscriptionPlanAsync(
            Guid subscriptionPlanId,
            CancellationToken cancellationToken)
        {
            var planId = new SubscriptionPlanId(subscriptionPlanId);
            var subscriptionPlan = await _subscriptionPlanRepository.GetByIdAsync(planId, cancellationToken);

            if (subscriptionPlan is null)
            {
                throw new InvalidOperationException($"Subscription plan with ID {planId.Value} not found.");
            }

            return subscriptionPlan;
        }

        private async Task<User> GetUserAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(new UserId(userId), cancellationToken);

            if (user is null)
            {
                throw new InvalidOperationException($"User with ID {userId} not found.");
            }

            return user;
        }

        private void ChangeUserSubscriptionPlan(
            User user,
            SubscriptionPlan subscriptionPlan)
        {
            var changeResult = user.ChangeSubscriptionPlan(subscriptionPlan);

            if (changeResult.IsFailure)
            {
                throw new InvalidOperationException(
                    $"Failed to change subscription plan for user with ID {user.Id.Value}. Error description: {changeResult.Error.Description}");
            }
        }

        private Transaction CreateTransaction(
            User user,
            SubscriptionPlan subscriptionPlan,
            Metadata? metadata)
        {
            var transactionId = new TransactionId(Guid.NewGuid());

            var transactionResult = Transaction.Create(
                transactionId,
                user,
                subscriptionPlan,
                timestamp: DateTime.UtcNow,
                metadata: metadata);

            if (transactionResult.IsFailure)
            {
                throw new InvalidOperationException(
                    $"Failed to create a transaction object. Error description: {transactionResult.Error.Description}");
            }

            return transactionResult.Value;
        }
    }
}
