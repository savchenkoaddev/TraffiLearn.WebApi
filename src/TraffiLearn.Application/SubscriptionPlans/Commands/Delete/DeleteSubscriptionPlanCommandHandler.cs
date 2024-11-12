using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.SubscriptionPlans.Commands.Delete
{
    internal sealed class DeleteSubscriptionPlanCommandHandler
        : IRequestHandler<DeleteSubscriptionPlanCommand, Result>
    {
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSubscriptionPlanCommandHandler(
            ISubscriptionPlanRepository subscriptionPlanRepository,
            IUnitOfWork unitOfWork)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DeleteSubscriptionPlanCommand request,
            CancellationToken cancellationToken)
        {
            SubscriptionPlanId subscriptionPlanId = new(
                request.SubscriptionPlanId.Value);

            var subscriptionPlan = await _subscriptionPlanRepository.GetByIdAsync(
                subscriptionPlanId,
                cancellationToken);

            if (subscriptionPlan is null)
            {
                return SubscriptionPlanErrors.NotFound;
            }

            await _subscriptionPlanRepository.DeleteAsync(subscriptionPlan);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
