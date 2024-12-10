using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.SubscriptionPlans.Commands.Create
{
    internal sealed class CreateSubscriptionPlanCommandHandler
        : IRequestHandler<CreateSubscriptionPlanCommand, Result<Guid>>
    {
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly Mapper<CreateSubscriptionPlanCommand, Result<SubscriptionPlan>> _requestMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSubscriptionPlanCommandHandler(
            ISubscriptionPlanRepository subscriptionPlanRepository,
            Mapper<CreateSubscriptionPlanCommand, Result<SubscriptionPlan>> requestMapper,
            IUnitOfWork unitOfWork)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _requestMapper = requestMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateSubscriptionPlanCommand request,
            CancellationToken cancellationToken)
        {
            var mappingResult = _requestMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return Result.Failure<Guid>(mappingResult.Error);
            }

            var subscriptionPlan = mappingResult.Value;

            await _subscriptionPlanRepository.InsertAsync(
                plan: subscriptionPlan,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(subscriptionPlan.Id.Value);
        }
    }
}
