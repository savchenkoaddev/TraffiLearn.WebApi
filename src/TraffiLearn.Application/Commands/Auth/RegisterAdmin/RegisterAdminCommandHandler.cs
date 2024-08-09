using MediatR;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Options;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Auth.RegisterAdmin
{
    internal sealed class RegisterAdminCommandHandler
        : IRequestHandler<RegisterAdminCommand, Result>
    {
        private readonly IUserManagementService _userManagementService;
        private readonly Mapper<RegisterAdminCommand, Result<User>> _commandMapper;
        private readonly AuthSettings _authSettings;

        public RegisterAdminCommandHandler(
            IUserManagementService userManagementService,
            Mapper<RegisterAdminCommand, Result<User>> commandMapper,
            IOptions<AuthSettings> authSettings)
        {
            _userManagementService = userManagementService;
            _commandMapper = commandMapper;
            _authSettings = authSettings.Value;
        }

        public async Task<Result> Handle(
            RegisterAdminCommand request,
            CancellationToken cancellationToken)
        {
            var mappingResult = _commandMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var userResult = await _userManagementService.GetAuthenticatedUserAsync(
                cancellationToken);

            if (userResult.IsFailure)
            {
                return userResult.Error;
            }

            var creator = userResult.Value;

            if (IsNotAllowedToCreateAdmins(creator))
            {
                return UserErrors.NotAllowedToPerformAction;
            }

            var newAdmin = mappingResult.Value;

            var createResult = await _userManagementService.CreateUserAsync(
                user: newAdmin,
                password: request.Password,
                cancellationToken);

            if (createResult.IsFailure)
            {
                return createResult.Error;
            }

            return Result.Success();
        }

        private bool IsNotAllowedToCreateAdmins(User user)
        {
            return user.Role < _authSettings.MinAllowedRoleToCreateAdminAccounts;
        }
    }
}
