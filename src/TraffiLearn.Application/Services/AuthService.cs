using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Application.Errors;
using TraffiLearn.Application.Options;
using TraffiLearn.Domain.Enums;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Services
{
    internal sealed class AuthService<TUser> : IAuthService<TUser>
        where TUser : class
    {
        private readonly SignInManager<TUser> _signInManager;
        private readonly UserManager<TUser> _userManager;
        private readonly LoginSettings _loginSettings;
        private readonly ILogger<AuthService<TUser>> _logger;

        public AuthService(
            SignInManager<TUser> signInManager,
            UserManager<TUser> userManager,
            IOptions<LoginSettings> loginSettings,
            ILogger<AuthService<TUser>> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _loginSettings = loginSettings.Value;
            _logger = logger;
        }

        public async Task<Result> AddIdentityUser(TUser identityUser, string password)
        {
            var result = await _userManager.CreateAsync(
                identityUser,
                password);

            if (!result.Succeeded)
            {
                _logger.LogCritical("Failed to create identity user.");

                return Error.InternalFailure();
            }

            return Result.Success();
        }

        public async Task<Result> AssignRoleToUser(TUser user, Role role)
        {
            var roleName = role.ToString();

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                _logger.LogCritical(
                    InternalErrors.RoleAssigningFailure(roleName).Description);

                return InternalErrors.RoleAssigningFailure(roleName);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteUser(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                return UserErrors.NotFound;
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogCritical("Failed to delete user from the identity storage. Errors: {Errors}", string.Join(',', result.Errors));

                return Error.InternalFailure();
            }

            return Result.Success();
        }

        public Result<Email> GetAuthenticatedUserEmail()
        {
            var userAuthenticated = _signInManager.Context.User.Identity.IsAuthenticated;

            if (!userAuthenticated)
            {
                _logger.LogError(InternalErrors.AuthorizationFailure.Description);

                return Result.Failure<Email>(InternalErrors.AuthorizationFailure);
            }

            var claimsEmail = _signInManager.Context.User.FindFirst(ClaimTypes.Email).Value;

            if (claimsEmail is null)
            {
                _logger.LogError(InternalErrors.ClaimMissing(nameof(Email)).Description);

                return Result.Failure<Email>(InternalErrors.ClaimMissing(nameof(Email)));
            }

            var emailCreateResult = Email.Create(claimsEmail);

            if (emailCreateResult.IsFailure)
            {
                _logger.LogError("Failed to create email due to unknown reasons. The registration request validation may have failed.");

                return Result.Failure<Email>(Error.InternalFailure());
            }

            return emailCreateResult.Value;
        }

        public Result<Guid> GetAuthenticatedUserId()
        {
            var userAuthenticated = _signInManager.Context.User.Identity.IsAuthenticated;

            if (!userAuthenticated)
            {
                _logger.LogError(InternalErrors.AuthorizationFailure.Description);

                return Result.Failure<Guid>(InternalErrors.AuthorizationFailure);
            }

            var claimsId = _signInManager.Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (claimsId is null)
            {
                var claimName = "id";

                _logger.LogError(InternalErrors.ClaimMissing(claimName).Description);

                return Result.Failure<Guid>(InternalErrors.ClaimMissing(claimName));
            }

            if (Guid.TryParse(claimsId, out Guid id))
            {
                return id;
            }

            _logger.LogError("Failed to parse id from to GUID. The id: {id}", claimsId);

            return Result.Failure<Guid>(Error.InternalFailure());
        }

        public async Task<Result<SignInResult>> PasswordLogin(
            TUser user,
            string password)
        {
            var canLogin = await _signInManager.CanSignInAsync(user);

            if (!canLogin)
            {
                return Result.Failure<SignInResult>(UserErrors.CannotLogin);
            }

            return await _signInManager.PasswordSignInAsync(
                user,
                password: password,
                isPersistent: _loginSettings.IsPersistent,
                lockoutOnFailure: _loginSettings.LockoutOnFailure);
        }

        public async Task<Result> RemoveRole(TUser user, Role role)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role.ToString());

            if (!result.Succeeded)
            {
                var errorsString = string.Join(',', result.Errors.Select(x => x.Description));

                _logger.LogError("Failed to remove role from user. Errors: {errors}", errorsString);

                return Error.InternalFailure();
            }

            return Result.Success();
        }
    }
}
