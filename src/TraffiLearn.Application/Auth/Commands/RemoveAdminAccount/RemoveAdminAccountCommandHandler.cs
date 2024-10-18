using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.RemoveAdminAccount
{
    internal sealed class RemoveAdminAccountCommandHandler
        : IRequestHandler<RemoveAdminAccountCommand, Result>
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveAdminAccountCommandHandler> _logger;

        public RemoveAdminAccountCommandHandler(
            IAuthenticatedUserService authenticatedUserService,
            IUserRepository userRepository,
            IIdentityService<ApplicationUser> identityService,
            IUnitOfWork unitOfWork,
            ILogger<RemoveAdminAccountCommandHandler> logger)
        {
            _authenticatedUserService = authenticatedUserService;
            _userRepository = userRepository;
            _identityService = identityService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            RemoveAdminAccountCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling RemoveAdminAccountCommand");

            var caller = await _authenticatedUserService
                .GetAuthenticatedUserAsync(cancellationToken);

            if (IsNotAllowedToRemoveAdmins(caller))
            {
                _logger.LogCritical("Caller tried to remove an admin account not having enough permissions. Caller role: {role}", caller.Role.ToString());

                throw new AuthorizationFailureException();
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
                _logger.LogCritical("Admin has not been found in identity storage, but has been found in repository. Critical data consistency issue.");

                throw new DataInconsistencyException();
            }

            Func<Task> transactionAction = async () =>
            {
                await _userRepository.DeleteAsync(admin);
                await _identityService.DeleteAsync(identityAdmin);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            };

            await _unitOfWork.ExecuteInTransactionAsync(
                transactionAction,
                cancellationToken: cancellationToken);

            _logger.LogInformation(
                "Succesfully removed the admin account. Username: {username}",
                admin.Username.Value);

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
