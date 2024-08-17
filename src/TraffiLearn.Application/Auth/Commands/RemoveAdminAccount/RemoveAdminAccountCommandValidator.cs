using FluentValidation;

namespace TraffiLearn.Application.Auth.Commands.RemoveAdminAccount
{
    internal sealed class RemoveAdminAccountCommandValidator
        : AbstractValidator<RemoveAdminAccountCommand>
    {
        public RemoveAdminAccountCommandValidator()
        {
            RuleFor(x => x.AdminId)
                .NotEmpty();
        }
    }
}
