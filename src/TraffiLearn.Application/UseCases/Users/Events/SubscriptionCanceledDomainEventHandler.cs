using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Shared.CanceledSubscriptions;
using TraffiLearn.Domain.Users.DomainEvents;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Events
{
    internal sealed class SubscriptionCanceledDomainEventHandler
        : INotificationHandler<SubscriptionCanceledDomainEvent>
    {
        private readonly Mapper<SubscriptionCanceledDomainEvent, Result<CanceledSubscription>> _mapper;
        private readonly ICanceledSubscriptionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SubscriptionCanceledDomainEventHandler> _logger;

        public SubscriptionCanceledDomainEventHandler(
            IServiceProvider serviceProvider,
            ILogger<SubscriptionCanceledDomainEventHandler> logger)
        {
            var sp = serviceProvider.CreateScope().ServiceProvider;

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
        }
    }
}
