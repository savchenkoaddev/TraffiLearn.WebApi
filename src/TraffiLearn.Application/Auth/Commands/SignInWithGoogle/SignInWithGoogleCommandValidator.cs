using FluentValidation;

namespace TraffiLearn.Application.Auth.Commands.SignInWithGoogle
{
    internal sealed class SignInWithGoogleCommandValidator
        : AbstractValidator<SignInWithGoogleCommand>
    {
        public SignInWithGoogleCommandValidator()
        {
            RuleFor(x => x.GoogleIdToken)
                .NotEmpty();
        }
    }
}
