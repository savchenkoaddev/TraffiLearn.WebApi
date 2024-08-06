using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Application.DTO.Auth;
using TraffiLearn.Application.Errors;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Auth.Login
{
    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IAuthService<ApplicationUser> _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(
            IAuthService<ApplicationUser> authService,
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            ITokenService tokenService,
            ILogger<LoginCommandHandler> logger)
        {
            _authService = authService;
            _userManager = userManager;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var identityUser = await _userManager.FindByEmailAsync(request.Email);

            if (identityUser is null)
            {
                return Result.Failure<LoginResponse>(UserErrors.NotFound);
            }

            var emailResult = Email.Create(request.Email);

            var user = await _userRepository.GetByEmailAsync(
                emailResult.Value,
                cancellationToken);

            if (user is null)
            {
                _logger.LogCritical(InternalErrors.DataConsistencyError.Description);

                return Result.Failure<LoginResponse>(InternalErrors.DataConsistencyError);
            }

            var loginResult = await _authService.PasswordLogin(
                user: identityUser,
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
