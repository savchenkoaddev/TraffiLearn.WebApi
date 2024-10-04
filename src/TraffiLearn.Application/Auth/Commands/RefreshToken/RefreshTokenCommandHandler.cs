using MediatR;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Auth.DTO;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.RefreshToken
{
    internal sealed class RefreshTokenCommandHandler
        : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
    {
        private readonly ITokenService _tokenService;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUserRepository _userRepository;

        public RefreshTokenCommandHandler(
            ITokenService tokenService,
            IIdentityService<ApplicationUser> identityService,
            IUserRepository userRepository)
        {

            _tokenService = tokenService;
            _identityService = identityService;
            _userRepository = userRepository;
        }

        public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var appUserResult = await _identityService
                .GetByAccessTokenAsync(request.AccessToken);

            if (appUserResult.IsFailure)
            {
                return Result.Failure<RefreshTokenResponse>(appUserResult.Error);
            }

            var applicationUser = appUserResult.Value;

            var refreshValidationResult = await _identityService
                .ValidateRefreshTokenAsync(applicationUser, request.RefreshToken);

            if (refreshValidationResult.IsFailure)
            {
                return Result.Failure<RefreshTokenResponse>(refreshValidationResult.Error);
            }

            var user = await _userRepository.GetByEmailAsync(
                Email.Create(applicationUser.Email).Value);

            if (user is null)
            {
                throw new DataInconsistencyException();
            }

            var accessToken = _tokenService.GenerateAccessToken(user);

            var refreshToken = _tokenService.GenerateRefreshToken();

            await _identityService.PopulateRefreshTokenAsync(applicationUser, refreshToken);

            return Result.Success(
                new RefreshTokenResponse(accessToken, refreshToken));
        }
    }
}
