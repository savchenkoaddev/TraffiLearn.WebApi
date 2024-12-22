using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Domain.Users;
using TraffiLearn.Domain.Users.DomainEvents;

namespace TraffiLearn.Application.UseCases.Users.Events
{
    internal sealed class SubscriptionRenewedDomainEventHandler
        : INotificationHandler<SubscriptionRenewedDomainEvent>
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public SubscriptionRenewedDomainEventHandler(IServiceProvider serviceProvider)
        {
            var sp = serviceProvider.CreateScope().ServiceProvider;

            _emailService = sp.GetRequiredService<IEmailService>();
            _userRepository = sp.GetRequiredService<IUserRepository>();
        }

        public async Task Handle(
            SubscriptionRenewedDomainEvent notification,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(
                new UserId(notification.UserId), cancellationToken);

            if (user is null)
            {
                throw new InvalidOperationException("User is not found.");
            }

            var userEmail = user.Email.Value;

            await _emailService.PublishPlanRenewedEmailAsync(
                userEmail,
                notification.PlanExpiresOn);
        }
    }
}
