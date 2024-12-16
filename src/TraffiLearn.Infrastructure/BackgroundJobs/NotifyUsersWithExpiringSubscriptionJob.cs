using Microsoft.Extensions.Logging;
using Quartz;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Domain.Users;

namespace TraffiLearn.Infrastructure.BackgroundJobs
{
    [DisallowConcurrentExecution]
    internal sealed class NotifyUsersWithExpiringSubscriptionJob : IJob
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<NotifyUsersWithExpiringSubscriptionJob> _logger;

        public NotifyUsersWithExpiringSubscriptionJob(
            IUserRepository userRepository,
            IEmailService emailService,
            ILogger<NotifyUsersWithExpiringSubscriptionJob> logger)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogDebug("Starting execution of NotifyUsersWithExpiringSubscriptionJob.");

            await NotifyUsersWithExpiringSubscription(7, context.CancellationToken);

            await NotifyUsersWithExpiringSubscription(3, context.CancellationToken);
            
            await NotifyUsersWithExpiringSubscription(1, context.CancellationToken);

            _logger.LogDebug("Execution of NotifyUsersWithExpiringSubscriptionJob is finished.");
        }

        private async Task NotifyUsersWithExpiringSubscription(
            int days,
            CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetWithExpiringSubscriptionPlanAsync(
                days: days,
                cancellationToken);

            if (!users.Any())
            {
                _logger.LogInformation("No users were found with subscription plans expiring in {Days} days.", days);

                return;
            }

            foreach (var user in users)
            {
                await _emailService.PublishPlanExpiryReminderEmailAsync(
                    user.Email.Value, user.PlanExpiresOn.Value, days);

                _logger.LogDebug("Sending email to {Email}.", user.Email.Value);
            }

            _logger.LogInformation("Sent {UserCount} emails.", users.ToList().Count);
            _logger.LogDebug("Successfully sent emails to users with subscription plans expiring in {Days} days.", days);
        }
    }
}
