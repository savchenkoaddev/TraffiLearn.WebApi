using MediatR;
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
        private readonly IConfirmationTokenGenerator _confirmationTokenGenerator;
        private readonly IEmailConfirmationLinkGenerator _confirmationLinkGenerator;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserCreatedDomainEventHandler> _logger;

        public UserCreatedDomainEventHandler(
            IEmailService emailService,
            IIdentityService<ApplicationUser> identityService,
            IConfirmationTokenGenerator confirmationTokenGenerator,
            IEmailConfirmationLinkGenerator confirmationLinkGenerator,
            IUserRepository userRepository,
            ILogger<UserCreatedDomainEventHandler> logger)
        {
            _emailService = emailService;
            _identityService = identityService;
            _confirmationTokenGenerator = confirmationTokenGenerator;
            _confirmationLinkGenerator = confirmationLinkGenerator;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Handle(
            UserCreatedDomainEvent notification,
            CancellationToken cancellationToken)
        {
            var identityUser = await _identityService.GetByEmailAsync(notification.Email);

            if (identityUser is null)
            {
                _logger.LogError(
                    "Identity user with the {email} is not found.",
                    notification.Email.Value);

                return;
            }

            var token = await _confirmationTokenGenerator.Generate(identityUser);

            var confirmationLink = _confirmationLinkGenerator.Generate(
                userId: notification.UserId.Value.ToString(),
                token: token);

            await _emailService.SendConfirmationEmail(
                recipientEmail: notification.Email.Value,
                confirmationLink: confirmationLink);
        }
    }
}
