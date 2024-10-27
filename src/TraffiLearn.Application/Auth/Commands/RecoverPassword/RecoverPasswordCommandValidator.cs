using FluentValidation;

namespace TraffiLearn.Application.Auth.Commands.RecoverPassword
{
    internal sealed class RecoverPasswordCommandValidator
        : AbstractValidator<RecoverPasswordCommand>
    {
        public RecoverPasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Token)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(30).WithMessage("Password must be no more than 30 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.")
                .NotEmpty().WithMessage("Password cannot be empty.");

            RuleFor(x => x.RepeatPassword)
                .Equal(x => x.NewPassword).WithMessage("Passwords do not match.")
                .NotEmpty();
        }
    }
}
