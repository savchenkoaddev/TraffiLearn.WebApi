using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.DTO.Auth;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Auth.Login
{
    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
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

            _logger.LogInformation("User found in identity service for email: {Email}", email);

            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

            if (user is null)
            {
                _logger.LogCritical("User not found in repository for email: {Email}. Critical data consistency issue.", email);

                throw new DataInconsistencyException();
            }

            _logger.LogInformation("User retrieved from repository for email: {Email}", email);

            var accessToken = _tokenService.GenerateAccessToken(user);

            _logger.LogInformation("Successfully generated access token for user: {UserId}", user.Id);

            return new LoginResponse(accessToken);
        }
    }
}
