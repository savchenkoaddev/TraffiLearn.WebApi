using FluentValidation;

namespace TraffiLearn.Application.UseCases.Auth.Commands.RemoveAdminAccount
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
