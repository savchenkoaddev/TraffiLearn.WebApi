using FluentValidation;

namespace TraffiLearn.Application.Auth.Commands.RefreshToken
{
    internal sealed class RefreshTokenCommandValidator 
        : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithMessage("Refresh token is required.");
        }
    }
}
