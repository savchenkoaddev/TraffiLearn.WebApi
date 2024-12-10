using FluentValidation;

namespace TraffiLearn.Application.UseCases.Auth.Commands.SignInWithGoogle
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
