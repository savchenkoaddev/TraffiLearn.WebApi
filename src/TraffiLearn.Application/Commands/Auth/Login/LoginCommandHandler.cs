using MediatR;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.DTO.Auth;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Auth.Login
{
    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IUserContextService<ApplicationUser> _authenticationService;

        public LoginCommandHandler(
            IUserContextService<ApplicationUser> authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<Result<LoginResponse>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var loginResult = await _authenticationService.LoginAsync(
                email: request.Email,
                password: request.Password,
                cancellationToken);

            if (loginResult.IsFailure)
            {
                return Result.Failure<LoginResponse>(loginResult.Error);
            }

            var response = new LoginResponse(loginResult.Value);

            return Result.Success(response);
        }
    }
}
