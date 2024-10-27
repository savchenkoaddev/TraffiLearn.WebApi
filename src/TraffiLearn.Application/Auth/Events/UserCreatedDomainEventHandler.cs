using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.DomainEvents;

namespace TraffiLearn.Application.Auth.Events
{
    internal sealed class UserCreatedDomainEventHandler
        : INotificationHandler<UserCreatedDomainEvent>
    {
        private readonly IEmailService _emailService;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserCreatedDomainEventHandler> _logger;

        public UserCreatedDomainEventHandler(
            IServiceProvider serviceProvider,
            ILogger<UserCreatedDomainEventHandler> logger)
        {
            var sp = serviceProvider.CreateScope().ServiceProvider;

            _emailService = sp.GetRequiredService<IEmailService>();
            _identityService = sp.GetRequiredService<IIdentityService<ApplicationUser>>();
            _userRepository = sp.GetRequiredService<IUserRepository>();

            _logger = logger;
        }

        public async Task Handle(
            UserCreatedDomainEvent notification,
            CancellationToken cancellationToken)
        {
            var identityUser = await _identityService.GetByEmailAsync(
                notification.Email);

            if (identityUser is null)
            {
                _logger.LogError(
                    "Identity user with the {email} is not found.",
                    notification.Email.Value);

                return;
            }

            string recipientEmail = notification.Email.Value;
            string userId = notification.UserId.Value.ToString();

            await _emailService.SendConfirmationEmailAsync(
                recipientEmail: recipientEmail,
                userId: userId,
                identityUser: identityUser);
        }
    }
}
