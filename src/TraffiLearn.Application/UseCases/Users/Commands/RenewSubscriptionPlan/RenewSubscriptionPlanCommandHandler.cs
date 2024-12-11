using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Users;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Commands.RenewSubscriptionPlan
{
    internal sealed class RenewSubscriptionPlanCommandHandler
        : IRequestHandler<RenewSubscriptionPlanCommand, Result<DateTime>>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RenewSubscriptionPlanCommandHandler(
            IUserContextService<Guid> userContextService,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<DateTime>> Handle(
            RenewSubscriptionPlanCommand request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService.GetAuthenticatedUserId());

            var user = await _userRepository.GetByIdWithPlanAsync(
                userId, cancellationToken);

            if (user is null)
            {
                throw new InvalidOperationException("Authenticated user is not found.");
            }

            var result = user.RenewPlan();

            if (result.IsFailure)
            {
                return Result.Failure<DateTime>(result.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(user.PlanExpiresOn!.Value);
        }
    }
}
