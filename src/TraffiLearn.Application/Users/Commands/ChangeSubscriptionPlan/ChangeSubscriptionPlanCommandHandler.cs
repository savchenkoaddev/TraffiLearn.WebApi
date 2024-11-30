using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Commands.ChangeSubscriptionPlan
{
    internal sealed class ChangeSubscriptionPlanCommandHandler
        : IRequestHandler<ChangeSubscriptionPlanCommand, Result>
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeSubscriptionPlanCommandHandler(
            IAuthenticatedUserService authenticatedUserService, 
            ISubscriptionPlanRepository subscriptionPlanRepository, 
            IUnitOfWork unitOfWork)
        {
            _authenticatedUserService = authenticatedUserService;
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            ChangeSubscriptionPlanCommand request, 
            CancellationToken cancellationToken)
        {
            var user = await _authenticatedUserService
                .GetAuthenticatedUserAsync(cancellationToken);

            var planId = new SubscriptionPlanId(request.SubscriptionPlanId.Value);

            var plan = await _subscriptionPlanRepository
                .GetByIdAsync(planId, cancellationToken);

            if (plan is null)
            {
                return SubscriptionPlanErrors.NotFound;
            }

            var result = user.ChangeSubscriptionPlan(plan);

            if (result.IsFailure)
            {
                return result.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
