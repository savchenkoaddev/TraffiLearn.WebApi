using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Services
{
    internal sealed class AuthService<TUser> : IAuthService<TUser>
        where TUser : class
    {
        private readonly SignInManager<TUser> _signInManager;
        private readonly ILogger<AuthService<TUser>> _logger;

        public AuthService(
            SignInManager<TUser> signInManager, 
            ILogger<AuthService<TUser>> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public Result<Email> GetCurrentUserEmail()
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
    }
}
