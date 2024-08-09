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
using TraffiLearn.Domain.Enums;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Services
{
    internal sealed class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly LoginSettings _loginSettings;
        private readonly Mapper<User, ApplicationUser> _userMapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            IOptions<LoginSettings> loginSettings,
            Mapper<User, ApplicationUser> userMapper,
            IUnitOfWork unitOfWork,
            ILogger<AuthService> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userRepository = userRepository;
            _loginSettings = loginSettings.Value;
            _userMapper = userMapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> CreateUser(
            User user,
            string password,
            CancellationToken cancellationToken = default)
        {
            if (await ExistsUser(user))
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

                var roleAssignmentResult = await AssignRoleToUser(
                    identityUser,
                    user.Role);

                if (roleAssignmentResult.IsFailure)
                {
                    return roleAssignmentResult.Error;
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Complete();
            }

            _logger.LogInformation("Successfully created a new user with {Email} email. ", identityUser.Email);

            return Result.Success();
        }

        public async Task<Result> AssignRoleToUser(
            ApplicationUser user,
            Role role)
        {
            var roleName = role.ToString();

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(
                    '\n',
                    InternalErrors.RoleAssigningFailure(roleName).Description,
                    GenerateErrorsString(result.Errors));

                _logger.LogCritical(errorMessage);

                return InternalErrors.RoleAssigningFailure(roleName);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteUser(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            var identityUser = await _userManager.FindByIdAsync(userId.Value.ToString());

            if (identityUser is null)
            {
                return UserErrors.NotFound;
            }

            var user = await _userRepository.GetByIdAsync(
                userId,
                cancellationToken);

            if (user is null)
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
                        "Failed to delete user from the identity storage. Errors: {Errors}",
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

            _logger.LogError("Failed to parse id to GUID. The id: {id}", claimsId);

            return Result.Failure<Guid>(Error.InternalFailure());
        }

        public async Task<Result<User>> GetCurrentUser(
            CancellationToken cancellationToken = default,
            params Expression<Func<User, object>>[] includeExpressions)
        {
            Result<Guid> userIdResult = GetAuthenticatedUserId();

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

        public async Task<Result<SignInResult>> PasswordLogin(
            ApplicationUser user,
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

        public async Task<Result> RemoveRole(
            ApplicationUser user,
            Role role)
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

        private string GenerateErrorsString(IEnumerable<IdentityError> errors)
        {
            return string.Join(',', errors.Select(error => error.Description));
        }

        private async Task<bool> ExistsUser(
            User user,
            CancellationToken cancellationToken = default)
        {
            return await _userRepository.ExistsAsync(
                user.Username,
                user.Email,
                cancellationToken);
        }

        public Task<Result> DeleteUser(UserId userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteUser(User user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<string>> Login(
            string email,
            string password,
            CancellationToken cancellationToken = default)
        {
            var emailResult = Email.Create(email);

            if (emailResult.IsFailure)
            {
                return Result.Failure<string>(emailResult.Error);
            }

            var identityUser = await _userManager.FindByEmailAsync(email);

            if (identityUser is null)
            {
                return Result.Failure<string>(UserErrors.NotFound);
            }

            var user = await _userRepository.GetByEmailAsync(
                emailResult.Value,
                cancellationToken);

            if (user is null)
            {
                _logger.LogCritical(InternalErrors.DataConsistencyError.Description);

                return Result.Failure<string>(InternalErrors.DataConsistencyError);
            }

            var canLogin = await _signInManager.CanSignInAsync(identityUser);

            if (!canLogin)
            {
                return Result.Failure<string>(UserErrors.CannotLogin);
            }

            var loginResult = await _signInManager.PasswordSignInAsync(
                identityUser,
                password: password,
                isPersistent: _loginSettings.IsPersistent,
                lockoutOnFailure: _loginSettings.LockoutOnFailure);

            if (!loginResult.Succeeded)
            {
                _logger.LogError("Failed to login through SignInManager. IsLockedOut: {isLockedOut}", loginResult.IsLockedOut);

                return Result.Failure<string>(UserErrors.InvalidCredentials);
            }

            var accessToken = _tokenService.GenerateAccessToken(user);

            _logger.LogInformation("Succesfully logged in. Access Token generated.");

            return accessToken;
        }
    }
}
