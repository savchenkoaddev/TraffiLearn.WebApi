using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Domain.Users;
using TraffiLearn.Domain.Users.DomainEvents;

namespace TraffiLearn.Application.UseCases.Users.Events
{
    internal sealed class SubscriptionChangedDomainEventHandler
        : INotificationHandler<SubscriptionChangedDomainEvent>
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public SubscriptionChangedDomainEventHandler(IServiceProvider serviceProvider)
        {
            var sp = serviceProvider.CreateScope().ServiceProvider;

            _emailService = sp.GetRequiredService<IEmailService>();
            _userRepository = sp.GetRequiredService<IUserRepository>();
        }

        public async Task Handle(
            SubscriptionChangedDomainEvent notification,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(
                new UserId(notification.UserId), cancellationToken);

            if (user is null)
            {
                throw new InvalidOperationException("User is not found.");
            }

            var userEmail = user.Email.Value;

            await _emailService.PublishPlanChangedEmailAsync(
                userEmail,
                notification.PlanExpiresOn);
        }
    }
}
