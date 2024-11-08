using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.SubscriptionPlans.Commands.Update
{
    internal sealed class UpdateSubscriptionPlanCommandHandler
        : IRequestHandler<UpdateSubscriptionPlanCommand, Result>
    {
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly Mapper<UpdateSubscriptionPlanCommand, Result<SubscriptionPlan>> _requestMapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSubscriptionPlanCommandHandler(
            ISubscriptionPlanRepository subscriptionPlanRepository,
            Mapper<UpdateSubscriptionPlanCommand, Result<SubscriptionPlan>> requestMapper,
            IUnitOfWork unitOfWork)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _requestMapper = requestMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateSubscriptionPlanCommand request,
            CancellationToken cancellationToken)
        {
            var mappingResult = _requestMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var subscriptionPlanId = new SubscriptionPlanId(
                request.SubscriptionPlanId.Value);

            var subscriptionPlan = await _subscriptionPlanRepository
                .GetByIdAsync(
                    subscriptionPlanId,
                    cancellationToken);

            if (subscriptionPlan is null)
            {
                return SubscriptionPlanErrors.NotFound;
            }

            var newSubscriptionPlan = mappingResult.Value;

            var updateResult = subscriptionPlan.Update(
                tier: newSubscriptionPlan.Tier,
                description: newSubscriptionPlan.Description,
                price: newSubscriptionPlan.Price,
                renewalPeriod: newSubscriptionPlan.RenewalPeriod,
                features: newSubscriptionPlan.Features.ToList());

            if (updateResult.IsFailure)
            {
                return updateResult.Error;
            }

            await _subscriptionPlanRepository.UpdateAsync(subscriptionPlan);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
