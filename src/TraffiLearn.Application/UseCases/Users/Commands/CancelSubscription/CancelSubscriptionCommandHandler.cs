using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Users;
using TraffiLearn.Domain.Users.CancelationReasons;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Commands.CancelSubscription
{
    internal sealed class CancelSubscriptionCommandHandler
        : IRequestHandler<CancelSubscriptionCommand, Result>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelSubscriptionCommandHandler(
            IUserContextService<Guid> userContextService, 
            IUserRepository userRepository, 
            IUnitOfWork unitOfWork)
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            CancelSubscriptionCommand request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService.GetAuthenticatedUserId());

            var user = await _userRepository.GetByIdWithPlanAsync(
                userId, cancellationToken);

            if (user is null)
            {
                throw new InvalidOperationException("Authenticated user is not found.");
            }

            var reasonResult = CancelationReason.Create(request.Reason);

            if (reasonResult.IsFailure)
            {
                return reasonResult.Error;
            }

            var reason = reasonResult.Value;

            var cancelationResult = user.CancelSubscription(reason);

            if (cancelationResult.IsFailure)
            {
                return cancelationResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
