using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Domain.Shared.CanceledSubscriptions;
using TraffiLearn.Domain.Users;
using TraffiLearn.Domain.Users.DomainEvents;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Events
{
    internal sealed class SubscriptionCanceledDomainEventHandler
        : INotificationHandler<SubscriptionCanceledDomainEvent>
    {
        private readonly IEmailService _emailService;
        private readonly Mapper<SubscriptionCanceledDomainEvent, Result<CanceledSubscription>> _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ICanceledSubscriptionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SubscriptionCanceledDomainEventHandler> _logger;

        public SubscriptionCanceledDomainEventHandler(
            IServiceProvider serviceProvider,
            ILogger<SubscriptionCanceledDomainEventHandler> logger)
        {
            var sp = serviceProvider.CreateScope().ServiceProvider;

            _emailService = sp.GetRequiredService<IEmailService>();

            _userRepository = sp.GetRequiredService<IUserRepository>();
            _repository = sp.GetRequiredService<ICanceledSubscriptionRepository>();
            _unitOfWork = sp.GetRequiredService<IUnitOfWork>();

            _mapper = sp.GetRequiredService<Mapper<
                SubscriptionCanceledDomainEvent, Result<CanceledSubscription>>>();

            _logger = logger;
        }

        public async Task Handle(
            SubscriptionCanceledDomainEvent notification,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(
                new UserId(notification.UserId), cancellationToken);

            if (user is null)
            {
                throw new InvalidOperationException(
                    "User with the canceled subscription could not be found.");
            }

            var canceledSubscriptionResult = _mapper.Map(notification);

            if (canceledSubscriptionResult.IsFailure)
            {
                throw new InvalidOperationException(
                    "Failed to map SubscriptionCanceledDomainEvent from the storage to the CanceledSubscription entity.");
            }

            var canceledSubscription = canceledSubscriptionResult.Value;

            await _repository.InsertAsync(
                canceledSubscription,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Succesfully inserted a CanceledSubscription Entity.");

            var userEmail = user.Email.Value;

            await _emailService.PublishPlanCancelationEmailAsync(userEmail);

            _logger.LogDebug("Succesfully sent plan cancelation notification email.");
        }
    }
}
