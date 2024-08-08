using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Transactions;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Errors;
using TraffiLearn.Application.Identity;
using TraffiLearn.Application.Options;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Users.DowngradeAccount
{
    internal sealed class DowngradeAccountCommandHandler
        : IRequestHandler<DowngradeAccountCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService<ApplicationUser> _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthSettings _authSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DowngradeAccountCommandHandler> _logger;

        public DowngradeAccountCommandHandler(
            IUserRepository userRepository,
            IAuthService<ApplicationUser> authService,
            UserManager<ApplicationUser> userManager,
            IOptions<AuthSettings> authSettings,
            IUnitOfWork unitOfWork,
            ILogger<DowngradeAccountCommandHandler> logger)
        {
            _userRepository = userRepository;
            _authService = authService;
            _userManager = userManager;
            _authSettings = authSettings.Value;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            DowngradeAccountCommand request,
            CancellationToken cancellationToken)
        {
            Result<Guid> downgraderIdResult = _authService.GetAuthenticatedUserId();

            if (downgraderIdResult.IsFailure)
            {
                return downgraderIdResult.Error;
            }

            UserId downgraderId = new(downgraderIdResult.Value);

            var downgrader = await _userRepository.GetByIdAsync(
                downgraderId,
                cancellationToken);

            if (downgrader is null)
            {
                _logger.LogCritical(InternalErrors.AuthenticatedUserNotFound.Description);

                return InternalErrors.AuthenticatedUserNotFound;
            }

            if (IsNotAllowedToDowngrade(downgrader))
            {
                return UserErrors.NotAllowedToPerformAction;
            }

            UserId affectedUserId = new(request.UserId.Value);

            var affectedUser = await _userRepository.GetByIdAsync(
                userId: affectedUserId,
                cancellationToken);

            if (affectedUser is null)
            {
                return UserErrors.NotFound;
            }

            if (CannotBeDowngraded(affectedUser))
            {
                return UserErrors.AccountCannotBeDowngraded;
            }

            var identityUser = await _userManager.FindByIdAsync(
                userId: affectedUser.Id.ToString());

            if (identityUser is null)
            {
                return InternalErrors.DataConsistencyError;
            }

            var previousRole = affectedUser.Role;
            var downgradeResult = affectedUser.DowngradeRole();

            if (downgradeResult.IsFailure)
            {
                return downgradeResult.Error;
            }

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _userRepository.UpdateAsync(affectedUser);

                var removeRoleResult = await _authService.RemoveRole(
                    identityUser,
                    previousRole);

                if (removeRoleResult.IsFailure)
                {
                    return removeRoleResult.Error;
                }

                var assigningResult = await _authService.AssignRoleToUser(
                    identityUser,
                    affectedUser.Role);

                if (assigningResult.IsFailure)
                {
                    return assigningResult.Error;
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Complete();
            }

            _logger.LogInformation(
                    "Successfully downgraded an account with email: {Email}. " +
                    "Previous role: {PreviousRole}. " +
                    "New role: {NewRole}",
                    affectedUser.Email.Value,
                    previousRole,
                    affectedUser.Role);

            return Result.Success();
        }

        private bool IsNotAllowedToDowngrade(User downgrader)
        {
            return downgrader.Role < _authSettings.MinimumAllowedRoleToDowngradeAccounts;
        }

        private bool CannotBeDowngraded(User user)
        {
            return user.Role < _authSettings.MinimumRoleForDowngrade;
        }
    }
}
