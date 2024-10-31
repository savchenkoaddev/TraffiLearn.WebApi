using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Auth.DTO;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Roles;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.SignInWithGoogle
{
    internal sealed class SignInWithGoogleCommandHandler
        : IRequestHandler<SignInWithGoogleCommand, Result<LoginResponse>>
    {
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly ITokenService _tokenService;
        private readonly IUsernameGenerator _usernameGenerator;
        private readonly Mapper<User, ApplicationUser> _identityUserMapper;
        private readonly IUnitOfWork _unitOfWork;

        public SignInWithGoogleCommandHandler(
            IGoogleAuthService googleAuthService,
            IUserRepository userRepository,
            IIdentityService<ApplicationUser> identityService,
            ITokenService tokenService,
            IUsernameGenerator usernameGenerator,
            Mapper<User, ApplicationUser> userMapper,
            IUnitOfWork unitOfWork)
        {
            _googleAuthService = googleAuthService;
            _userRepository = userRepository;
            _identityService = identityService;
            _tokenService = tokenService;
            _usernameGenerator = usernameGenerator;
            _identityUserMapper = userMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<LoginResponse>> Handle(
            SignInWithGoogleCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _googleAuthService.ValidateIdTokenAsync(
                token: request.GoogleIdToken);

            if (validationResult.IsFailure)
            {
                return Result.Failure<LoginResponse>(validationResult.Error);
            }

            var email = validationResult.Value;

            var user = await _userRepository.GetByEmailAsync(
                email, cancellationToken);

            bool emptyFirstTimeSignInPassword = request.FirstTimeSignInPassword is null;

            if (user is null && emptyFirstTimeSignInPassword)
            {
                return Result.Failure<LoginResponse>(UserErrors.FirstTimeUserSignInPasswordEmpty);
            }

            var identityUser = await _identityService.GetByEmailAsync(email);

            if (user is null)
            {
                if (identityUser is not null)
                {
                    throw new DataInconsistencyException();
                }

                var randomUsername = _usernameGenerator.Generate();

                user = User.Create(
                    new UserId(Guid.NewGuid()),
                    email,
                    randomUsername,
                    role: Role.RegularUser).Value;

                identityUser = _identityUserMapper.Map(user);

                await _userRepository.InsertAsync(user, cancellationToken);

                await _identityService.CreateAsync(
                    identityUser,
                    password: request.FirstTimeSignInPassword!);

                try
                {
                    await _identityService.AddToRoleAsync(
                        identityUser,
                        roleName: Role.RegularUser.ToString());

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
                catch (Exception)
                {
                    await _identityService.DeleteAsync(identityUser);

                    throw;
                }
            }

            var refreshToken = _tokenService.GenerateRefreshToken();
            var accessToken = _tokenService.GenerateAccessToken(user);

            await _identityService.PopulateRefreshTokenAsync(
                identityUser,
                refreshToken: refreshToken);

            return new LoginResponse(
                AccessToken: accessToken,
                RefreshToken: refreshToken);
        }
    }
}
