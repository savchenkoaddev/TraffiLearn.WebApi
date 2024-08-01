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

namespace TraffiLearn.Application.Commands.Auth.Login
{
    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginCommandHandler> _logger;
        private readonly LoginSettings _loginSettings;

        public LoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            ILogger<LoginCommandHandler> logger,
            IOptions<LoginSettings> loginSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
            _loginSettings = loginSettings.Value;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return Result.Failure<LoginResponse>(UserErrors.NotFound);
            }

            var canLogin = await _signInManager.CanSignInAsync(user);

            if (!canLogin)
            {
                return Result.Failure<LoginResponse>(UserErrors.CannotLogin);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(
                user,
                password: request.Password,
                isPersistent: _loginSettings.IsPersistent,
                lockoutOnFailure: _loginSettings.LockoutOnFailure);

            if (!signInResult.Succeeded)
            {
                return Result.Failure<LoginResponse>(UserErrors.InvalidCredentials);
            }

            var accessToken = _tokenService.GenerateAccessToken(user);

            _logger.LogInformation(
                "Succesfully logged in. Access Token: {AccessToken}", accessToken.Take(5).ToString());

            var response = new LoginResponse(accessToken);

            return Result.Success(response);
        }
    }
}
