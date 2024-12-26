using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.Domain.Transactions;
using TraffiLearn.Domain.Transactions.Metadatas;
using TraffiLearn.Domain.Users;

namespace TraffiLearn.Application.Webhooks.Stripe.Events.RenewSubscriptionCompleted
{
    internal sealed class RenewSubscriptionCompletedEventHandler
        : INotificationHandler<RenewSubscriptionCompletedEvent>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RenewSubscriptionCompletedEventHandler> _logger;

        public RenewSubscriptionCompletedEventHandler(
            ITransactionRepository transactionRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<RenewSubscriptionCompletedEventHandler> logger)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(
            RenewSubscriptionCompletedEvent notification,
            CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(
                notification.UserId,
                cancellationToken);

            RenewUserSubscriptionPlan(user);

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

            var userPlan = user.SubscriptionPlan;

            if (userPlan is null)
            {
                throw new InvalidOperationException($"Subscription plan is null. User ID: {user.Id.Value}");
            }

            var transaction = CreateTransaction(
                user, userPlan, metadata);

            await _transactionRepository.InsertAsync(
                transaction,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Subscription plan for user {userId} successfully renewed.",
                user.Id.Value);
        }

        private async Task<User> GetUserAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdWithPlanAsync(new UserId(userId), cancellationToken);

            if (user is null)
            {
                throw new InvalidOperationException($"User with ID {userId} not found.");
            }

            return user;
        }

        private void RenewUserSubscriptionPlan(User user)
        {
            var renewResult = user.RenewPlan();

            if (renewResult.IsFailure)
            {
                throw new InvalidOperationException(
                    $"Failed to renew subscription plan for user with ID {user.Id.Value}. Error description: {renewResult.Error.Description}");
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
