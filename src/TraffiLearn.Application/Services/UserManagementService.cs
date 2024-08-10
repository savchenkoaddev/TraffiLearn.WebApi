using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Transactions;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Errors;
using TraffiLearn.Application.Identity;
using TraffiLearn.Application.Options;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Services
{
    internal sealed class UserManagementService : IUserManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IdentityRoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<User, ApplicationUser> _userMapper;
        private readonly AuthSettings _authSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserManagementService> _logger;

        public UserManagementService(
            UserManager<ApplicationUser> userManager,
            IdentityRoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IUserRepository userRepository,
            Mapper<User, ApplicationUser> userMapper,
            IOptions<AuthSettings> authSettings,
            IUnitOfWork unitOfWork,
            ILogger<UserManagementService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _userMapper = userMapper;
            _authSettings = authSettings.Value;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> UpdateIdentityUserRoleAsync(
            User user,
            CancellationToken cancellationToken = default)
        {
            var identityUser = await _userManager.FindByIdAsync(user.Id.Value.ToString());

            if (identityUser is null)
            {
                _logger.LogCritical(InternalErrors.DataConsistencyError.Description);

                return InternalErrors.DataConsistencyError;
            }

            var previousRole = await _roleManager.GetRoleNameAsync(new IdentityRole(user.Role.ToString()));

            if (previousRole is null)
            {
                _logger.LogCritical("Failed to get role name through RoleManager, even though the identity user if found.");

                return Error.InternalFailure();
            }

            if (previousRole == user.Role.ToString())
            {
                return Result.Success();
            }

            using (var transaction = new TransactionScope(
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var removeResult = await _userManager.RemoveFromRoleAsync(identityUser, previousRole);

                if (!removeResult.Succeeded)
                {
                    var errorsString = GenerateErrorsString(removeResult.Errors);

                    _logger.LogError("Failed to remove role from user. Errors: {errors}", errorsString);

                    return Error.InternalFailure();
                }

                var newRole = user.Role.ToString();

                var addRoleResult = await _userManager.AddToRoleAsync(identityUser, newRole);

                if (!addRoleResult.Succeeded)
                {
                    var errorMessage = string.Join(
                        '\n',
                        InternalErrors.RoleAssigningFailure(newRole).Description,
                        GenerateErrorsString(addRoleResult.Errors));

                    _logger.LogCritical(errorMessage);

                    return InternalErrors.RoleAssigningFailure(newRole);
                }

                transaction.Complete();
            }

            return Result.Success();
        }

        public async Task<Result> CreateUserAsync(
            User user,
            string password,
            CancellationToken cancellationToken = default)
        {
            var userExists = await UserExists(
                user,
                cancellationToken);

            if (userExists)
            {
                return UserErrors.AlreadyRegistered;
            }

            var identityUser = _userMapper.Map(user);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _userRepository.AddAsync(
                    user,
                    cancellationToken);

                var result = await _userManager.CreateAsync(
                    identityUser,
                    password);

                if (!result.Succeeded)
                {
                    var errors = GenerateErrorsString(result.Errors);

                    _logger.LogCritical("Failed to create identity user. Errors: {errors}", errors);

                    return Error.InternalFailure();
                }

                var roleAssignmentResult = await UpdateIdentityUserRoleAsync(
                    user,
                    cancellationToken);

                if (roleAssignmentResult.IsFailure)
                {
                    return roleAssignmentResult.Error;
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Complete();
            }

            _logger.LogInformation("Successfully created a new admin user with {Email} email.", identityUser.Email);

            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(
            User user,
            CancellationToken cancellationToken = default)
        {
            var identityUser = await _userManager.FindByIdAsync(user.Id.Value.ToString());

            if (identityUser is null)
            {
                _logger.LogCritical(InternalErrors.DataConsistencyError.Description);

                return InternalErrors.DataConsistencyError;
            }

            using (var transaction = new TransactionScope(
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var identityDeleteResult = await _userManager.DeleteAsync(identityUser);

                if (!identityDeleteResult.Succeeded)
                {
                    _logger.LogCritical(
                        "Failed to delete a user from the identity storage. Errors: {Errors}",
                        GenerateErrorsString(identityDeleteResult.Errors));

                    return Error.InternalFailure();
                }

                await _userRepository.DeleteAsync(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Complete();
            }

            _logger.LogInformation("Succesfully deleted user. Role: {role}. Email: {email}", user.Role, user.Email);

            return Result.Success();
        }

        public async Task<Result> EnsureUserCanPerformAction(
            Predicate<User> predicate,
            CancellationToken cancellationToken = default)
        {
            var authorizationResult = await GetAuthenticatedUserAsync(cancellationToken);

            if (authorizationResult.IsFailure)
            {
                return authorizationResult.Error;
            }

            var caller = authorizationResult.Value;

            if (predicate(caller))
            {
                return UserErrors.NotAllowedToPerformAction;
            }

            return Result.Success();
        }

        public async Task<Result<User>> GetAuthenticatedUserAsync(
            CancellationToken cancellationToken = default,
            params Expression<Func<User, object>>[] includeExpressions)
        {
            var userIdResult = await GetAuthenticatedUserId();

            if (userIdResult.IsFailure)
            {
                return Result.Failure<User>(userIdResult.Error);
            }

            UserId userId = new(userIdResult.Value);

            var user = await _userRepository.GetByIdAsync(
                userId,
                cancellationToken,
                includeExpressions);

            if (user is null)
            {
                _logger.LogCritical(InternalErrors.AuthenticatedUserNotFound.Description);

                return Result.Failure<User>(InternalErrors.AuthenticatedUserNotFound);
            }

            return user;
        }

        public async Task<Result<Guid>> GetAuthenticatedUserId()
        {
            var userClaims = _signInManager.Context.User;

            if (userClaims.Identity is null || !userClaims.Identity.IsAuthenticated)
            {
                _logger.LogError("User is not authenticated. Identity: {Identity}", userClaims.Identity);

                return Result.Failure<Guid>(InternalErrors.AuthorizationFailure);
            }

            var idClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim is null)
            {
                var claimName = "id";
                _logger.LogError("Claim missing. Claim name: {ClaimName}. User Claims: {UserClaims}", claimName, userClaims.Claims);

                return Result.Failure<Guid>(InternalErrors.ClaimMissing(claimName));
            }

            var idString = idClaim.Value;

            if (Guid.TryParse(idString, out Guid id))
            {
                _logger.LogInformation("Successfully parsed user ID. User ID: {UserId}", id);

                return id;
            }

            _logger.LogError("Failed to parse ID to GUID. ID: {IdString}", idString);

            await Task.CompletedTask;

            return Result.Failure<Guid>(Error.InternalFailure());
        }

        private async Task<bool> UserExists(
            User user,
            CancellationToken cancellationToken = default)
        {
            return await _userRepository.ExistsAsync(
                user.Username,
                user.Email,
                cancellationToken);
        }

        private string GenerateErrorsString(IEnumerable<IdentityError> errors)
        {
            return string.Join(',', errors.Select(error => error.Description));
        }
    }
}
