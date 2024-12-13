using MediatR;
using System.Numerics;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Abstractions.Payments;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Commands.ChangeSubscriptionPlan
{
    internal sealed class ChangeSubscriptionPlanCommandHandler
        : IRequestHandler<ChangeSubscriptionPlanCommand, Result>
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeSubscriptionPlanCommandHandler(
            IAuthenticatedUserService authenticatedUserService,
            ISubscriptionPlanRepository subscriptionPlanRepository,
            IPaymentService paymentService,
            IUnitOfWork unitOfWork)
        {
            _authenticatedUserService = authenticatedUserService;
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _paymentService = paymentService;
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

            var createCheckoutSessionRequest = GetCreateCheckoutSessionRequest(plan);

            var sessionUri = await _paymentService.CreateCheckoutSessionAsync(
                createCheckoutSessionRequest);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(sessionUri);
        }

        private CreateCheckoutSessionRequest GetCreateCheckoutSessionRequest(
            SubscriptionPlan plan)
        {
            return new CreateCheckoutSessionRequest(
                ProductName: plan.Tier.Value,
                Amount: plan.Price.Amount,
                Currency: plan.Price.Currency.ToString(),
                Quantity: 1,
                PaymentMode: "subscription");
        }
    }
}
