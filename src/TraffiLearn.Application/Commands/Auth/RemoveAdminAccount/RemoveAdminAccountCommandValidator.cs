using FluentValidation;

namespace TraffiLearn.Application.Commands.Auth.RemoveAdminAccount
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
