using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Transactions;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
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
        private readonly IUserManagementService _userManagementService;
        private readonly AuthSettings _authSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DowngradeAccountCommandHandler> _logger;

        public DowngradeAccountCommandHandler(
            IUserRepository userRepository,
            IUserManagementService userManagementService,
            IOptions<AuthSettings> authSettings,
            IUnitOfWork unitOfWork,
            ILogger<DowngradeAccountCommandHandler> logger)
        {
            _userRepository = userRepository;
            _userManagementService = userManagementService;
            _authSettings = authSettings.Value;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            DowngradeAccountCommand request,
            CancellationToken cancellationToken)
        {
            var downgraderResult = await _userManagementService.GetAuthenticatedUserAsync(
                cancellationToken);

            if (downgraderResult.IsFailure)
            {
                return downgraderResult.Error;
            }

            var downgrader = downgraderResult.Value;

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

            var previousRole = affectedUser.Role;
            var downgradeResult = affectedUser.DowngradeRole();

            if (downgradeResult.IsFailure)
            {
                return downgradeResult.Error;
            }

            using (var transaction = new TransactionScope(
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var assignRoleResult = await _userManagementService.UpdateIdentityUserRoleAsync(
                    affectedUser,
                    cancellationToken);

                if (assignRoleResult.IsFailure)
                {
                    return assignRoleResult.Error;
                }

                await _userRepository.UpdateAsync(affectedUser);
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
            return downgrader.Role < _authSettings.MinAllowedRoleToDowngradeAccounts;
        }

        private bool CannotBeDowngraded(User user)
        {
            return user.Role < _authSettings.MinRoleForDowngrade;
        }
    }
}
