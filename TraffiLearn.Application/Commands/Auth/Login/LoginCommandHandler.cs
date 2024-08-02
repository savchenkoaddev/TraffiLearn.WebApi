using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Application.DTO.Auth;
using TraffiLearn.Application.Identity;
using TraffiLearn.Application.Options;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Auth.Login
{
    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IAuthService<ApplicationUser> _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(
            IAuthService<ApplicationUser> authService,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            ILogger<LoginCommandHandler> logger)
        {
            _authService = authService;
            _userManager = userManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return Result.Failure<LoginResponse>(UserErrors.NotFound);
            }

            var loginResult = await _authService.PasswordLogin(
                user: user,
                password: request.Password);

            if (loginResult.IsFailure)
            {
                return Result.Failure<LoginResponse>(loginResult.Error);
            }

            if (!loginResult.Value.Succeeded)
            {
                return Result.Failure<LoginResponse>(UserErrors.InvalidCredentials);
            }

            var accessToken = _tokenService.GenerateAccessToken(user);

            _logger.LogInformation("Succesfully logged in. Access Token generated.");

            var response = new LoginResponse(accessToken);

            return Result.Success(response);
        }
    }
}
