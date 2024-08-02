using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Application.DTO.Auth;
using TraffiLearn.Application.Options;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Services
{
    internal sealed class AuthService<TUser> : IAuthService<TUser>
        where TUser : class
    {
        private readonly SignInManager<TUser> _signInManager;
        private readonly LoginSettings _loginSettings;
        private readonly ILogger<AuthService<TUser>> _logger;

        public AuthService(
            SignInManager<TUser> signInManager,
            IOptions<LoginSettings> loginSettings,
            ILogger<AuthService<TUser>> logger)
        {
            _signInManager = signInManager;
            _loginSettings = loginSettings.Value;
            _logger = logger;
        }

        public Result<Email> GetAuthenticatedUserEmail()
        {
            var userAuthenticated = _signInManager.Context.User.Identity.IsAuthenticated;

            if (!userAuthenticated)
            {
                _logger.LogError("The user is not authenticated. This is probably due to some authorization failures.");

                return Result.Failure<Email>(Error.InternalFailure());
            }

            var claimsEmail = _signInManager.Context.User.FindFirst(ClaimTypes.Email).Value;

            if (claimsEmail is null)
            {
                _logger.LogError("Couldn't fetch the email from http context. This is probably due to the token generation issues.");

                return Result.Failure<Email>(Error.InternalFailure());
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
                _logger.LogError("The user is not authenticated. This is probably due to some authorization failures.");

                return Result.Failure<Guid>(Error.InternalFailure());
            }

            var claimsId = _signInManager.Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (claimsId is null)
            {
                _logger.LogError("Couldn't fetch the id from http context. This is probably due to the token generation issues.");

                return Result.Failure<Guid>(Error.InternalFailure());
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
    }
}
