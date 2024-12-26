using MediatR;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Abstractions.Payments;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.Domain.Users;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Commands.RequestRenewSubscriptionPlan
{
    internal sealed class RequestRenewSubscriptionPlanCommandHandler
        : IRequestHandler<RequestRenewSubscriptionPlanCommand, Result<Uri>>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly IPaymentService _paymentService;

        public RequestRenewSubscriptionPlanCommandHandler(
            IUserContextService<Guid> userContextService,
            IUserRepository userRepository,
            IPaymentService paymentService)
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
            _paymentService = paymentService;
        }

        public async Task<Result<Uri>> Handle(
            RequestRenewSubscriptionPlanCommand request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService.GetAuthenticatedUserId());

            var user = await _userRepository.GetByIdWithPlanAsync(
                userId, cancellationToken);

            if (user is null)
            {
                throw new InvalidOperationException("Authenticated user is not found.");
            }

            var canUserRenewPlan = user.CanRenewPlan();

            if (canUserRenewPlan.IsFailure)
            {
                return Result.Failure<Uri>(canUserRenewPlan.Error);
            }

            var createCheckoutSessionRequest = GetCreateCheckoutSessionRequest(user.SubscriptionPlan!);

            var metadata = GetMetadataForRequest(user.Id);

            var sessionUri = await _paymentService.CreateCheckoutSessionAsync(
                createCheckoutSessionRequest, metadata);

            return Result.Success(sessionUri);
        }

        private static Dictionary<string, string> GetMetadataForRequest(
            UserId userId)
        {
            return new Dictionary<string, string>
            {
                { "userId", userId.Value.ToString() }
            };
        }

        private static CreateCheckoutSessionRequest GetCreateCheckoutSessionRequest(
            SubscriptionPlan plan)
        {
            return new CreateCheckoutSessionRequest(
                ProductName: plan.Tier.Value,
                Amount: plan.Price.Amount,
                Currency: plan.Price.Currency.ToString(),
                Quantity: 1,
                PaymentMode: "payment",
                Action: CheckoutSessionAction.RenewPlan);
        }
    }
}
