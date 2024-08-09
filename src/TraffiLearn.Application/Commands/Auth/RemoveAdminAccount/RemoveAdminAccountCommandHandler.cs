using MediatR;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Options;
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
        private readonly IUserManagementService _userManagementService;
        private readonly AuthSettings _authSettings;

        public RemoveAdminAccountCommandHandler(
            IUserRepository userRepository,
            IUserManagementService userManagementService,
            IOptions<AuthSettings> identitySettings)
        {
            _userRepository = userRepository;
            _userManagementService = userManagementService;
            _authSettings = identitySettings.Value;
        }

        public async Task<Result> Handle(
            RemoveAdminAccountCommand request,
            CancellationToken cancellationToken)
        {
            var removerResult = await _userManagementService.GetAuthenticatedUserAsync(
                cancellationToken);

            if (removerResult.IsFailure)
            {
                return removerResult.Error;
            }

            var remover = removerResult.Value;

            if (IsNotAllowedToRemoveAdmins(remover))
            {
                return UserErrors.NotAllowedToPerformAction;
            }

            UserId adminId = new(request.AdminId.Value);

            var admin = await _userRepository.GetByIdAsync(
                adminId,
                cancellationToken);

            if (admin is null)
            {
                return UserErrors.NotFound;
            }

            if (IsNotAdmin(admin))
            {
                return UserErrors.RemovedAccountIsNotAdminAccount;
            }

            var removeResult = await _userManagementService.DeleteUserAsync(
                admin,
                cancellationToken);

            if (removeResult.IsFailure)
            {
                return removeResult.Error;
            }

            return Result.Success();
        }

        private bool IsNotAdmin(User user)
        {
            return user.Role != Role.Admin;
        }

        private bool IsNotAllowedToRemoveAdmins(User remover)
        {
            return remover.Role < _authSettings.MinAllowedRoleToRemoveAdminAccounts;
        }
    }
}
