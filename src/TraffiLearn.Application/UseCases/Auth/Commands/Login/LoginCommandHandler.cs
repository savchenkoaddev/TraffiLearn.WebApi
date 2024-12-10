using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.UseCases.Auth.DTO;
using TraffiLearn.Application.UseCases.Users.Identity;
using TraffiLearn.Domain.Users;
using TraffiLearn.Domain.Users.Emails;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Auth.Commands.Login
{
    internal sealed class LoginCommandHandler
        : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(
            IIdentityService<ApplicationUser> identityService,
            IUserRepository userRepository,
            ITokenService tokenService,
            ILogger<LoginCommandHandler> logger)
        {
            _identityService = identityService;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<Result<LoginResponse>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling LoginCommand for email: {Email}", request.Email);

            var emailResult = Email.Create(request.Email);

            if (emailResult.IsFailure)
            {
                _logger.LogWarning("Invalid email format: {Email}", request.Email);

                return Result.Failure<LoginResponse>(emailResult.Error);
            }

            var email = emailResult.Value;

            var identityUser = await _identityService.GetByEmailAsync(email);

            if (identityUser is null)
            {
                return Result.Failure<LoginResponse>(UserErrors.NotFound);
            }

            var loginResult = await _identityService.LoginAsync(
                identityUser,
                password: request.Password);

            if (loginResult.IsFailure)
            {
                return Result.Failure<LoginResponse>(loginResult.Error);
            }

            _logger.LogInformation("User found in identity service for email: {Email}", email.Value);

            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

            if (user is null)
            {
                _logger.LogCritical("User not found in repository for email: {Email}. Critical data consistency issue.", email.Value);

                throw new DataInconsistencyException();
            }

            _logger.LogInformation("User retrieved from repository for email: {Email}", email.Value);

            if (!user.IsEmailConfirmed)
            {
                return Result.Failure<LoginResponse>(
                    UserErrors.EmailNotConfirmed);
            }

            var refreshToken = _tokenService.GenerateRefreshToken();

            await _identityService.PopulateRefreshTokenAsync(identityUser, refreshToken);

            _logger.LogInformation("Successfully generated and populated refresh token for user: {UserId}", user.Id.Value);

            var accessToken = _tokenService.GenerateAccessToken(user);

            _logger.LogInformation("Successfully generated access token for user: {UserId}", user.Id.Value);

            return new LoginResponse(
                accessToken, refreshToken);
        }
    }
}
