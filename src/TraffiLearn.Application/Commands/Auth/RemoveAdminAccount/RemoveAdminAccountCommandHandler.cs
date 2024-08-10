using MediatR;
using Microsoft.Extensions.Logging;
using System.Transactions;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Errors;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Enums;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Auth.RemoveAdminAccount
{
    internal sealed class RemoveAdminAccountCommandHandler
        : IRequestHandler<RemoveAdminAccountCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveAdminAccountCommandHandler> _logger;

        public RemoveAdminAccountCommandHandler(
            IUserRepository userRepository,
            IIdentityService<ApplicationUser> identityService,
            IUserContextService<Guid> userContextService,
            IUnitOfWork unitOfWork,
            ILogger<RemoveAdminAccountCommandHandler> logger)
        {
            _userRepository = userRepository;
            _identityService = identityService;
            _userContextService = userContextService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            RemoveAdminAccountCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling RemoveAdminAccountCommand");

            var callerId = _userContextService.FetchAuthenticatedUserId();

            _logger.LogInformation("Succesfully fetched caller id. Caller ID: {CallerId}", callerId);

            var caller = await _userRepository.GetByIdAsync(
                new UserId(callerId),
                cancellationToken);

            if (caller is null)
            {
                _logger.LogCritical(InternalErrors.AuthorizationFailure.Description);

                return InternalErrors.AuthorizationFailure;
            }

            if (IsNotAllowedToRemoveAdmins(caller))
            {
                _logger.LogWarning("Caller tried to remove an admin account not having enough permissions. Caller role: {role}", caller.Role.ToString());

                return UserErrors.NotAllowedToPerformAction;
            }

            var admin = await _userRepository.GetByIdAsync(
                new UserId(request.AdminId.Value),
                cancellationToken);

            if (admin is null)
            {
                return UserErrors.NotFound;
            }

            if (IsNotAdmin(admin))
            {
                return UserErrors.RemovedAccountIsNotAdminAccount;
            }

            var identityAdmin = await _identityService.GetByEmailAsync(admin.Email);

            if (identityAdmin is null)
            {
                _logger.LogCritical(InternalErrors.DataConsistencyError.Description);

                return InternalErrors.DataConsistencyError;
            }

            using (var transaction = new TransactionScope(
                TransactionScopeAsyncFlowOption.Enabled))
            {
                await _userRepository.DeleteAsync(admin);

                await _identityService.DeleteAsync(identityAdmin);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Complete();
            }

            _logger.LogInformation(
                "Succesfully removed the admin account. Username: {username}",
                admin.Username);

            return Result.Success();
        }

        private bool IsNotAdmin(User user)
        {
            return user.Role != Role.Admin;
        }

        private bool IsNotAllowedToRemoveAdmins(User remover)
        {
            return remover.Role < Role.Owner;
        }
    }
}
