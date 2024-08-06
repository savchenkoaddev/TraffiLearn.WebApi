using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Transactions;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Errors;
using TraffiLearn.Application.Identity;
using TraffiLearn.Application.Options;
using TraffiLearn.Domain.Enums;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Auth.RemoveAdminAccount
{
    internal sealed class RemoveAdminAccountCommandHandler
        : IRequestHandler<RemoveAdminAccountCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService<ApplicationUser> _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthSettings _authSettings;
        private readonly ILogger<RemoveAdminAccountCommandHandler> _logger;

        public RemoveAdminAccountCommandHandler(
            IUserRepository userRepository,
            IAuthService<ApplicationUser> authService,
            IUnitOfWork unitOfWork,
            IOptions<AuthSettings> authSettings,
            ILogger<RemoveAdminAccountCommandHandler> logger)
        {
            _userRepository = userRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
            _authSettings = authSettings.Value;
            _logger = logger;
        }

        public async Task<Result> Handle(
            RemoveAdminAccountCommand request,
            CancellationToken cancellationToken)
        {
            // Transaction is required due to features of UserManager.
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                Result<Guid> removerIdResult = _authService.GetAuthenticatedUserId();

                if (removerIdResult.IsFailure)
                {
                    return removerIdResult.Error;
                }

                var removerId = removerIdResult.Value;

                var remover = await _userRepository.GetByIdAsync(
                    removerId,
                    cancellationToken);

                if (remover is null)
                {
                    _logger.LogCritical(InternalErrors.AuthenticatedUserNotFound.Description);

                    return InternalErrors.AuthenticatedUserNotFound;
                }

                if (remover.Role < _authSettings.MinimumAllowedRoleToRemoveAdminAccounts)
                {
                    return UserErrors.NotAllowedToRemoveAdmins;
                }

                var admin = await _userRepository.GetByIdAsync(
                    userId: request.AdminId.Value,
                    cancellationToken);

                if (admin is null)
                {
                    return UserErrors.NotFound;
                }

                if (admin.Role != Role.Admin)
                {
                    return UserErrors.RemovedAccountIsNotAdminAccount;
                }

                await _userRepository.DeleteAsync(admin);

                // This call persists changes to the database regardless of UoW
                var deleteResult = await _authService.DeleteUser(admin.Id);

                if (deleteResult.IsFailure)
                {
                    return deleteResult.Error;
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Successfully removed an admin account with {Email} email.", admin.Email.Value);

                transaction.Complete();
            }

            return Result.Success();
        }
    }
}
