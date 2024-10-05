using FluentValidation;

namespace TraffiLearn.Application.ServiceCenters.Commands.Delete
{
    internal sealed class DeleteServiceCenterCommandValidator
        : AbstractValidator<DeleteServiceCenterCommand>
    {
        public DeleteServiceCenterCommandValidator()
        {
            RuleFor(x => x.ServiceCenterId)
                .NotEmpty();
        }
    }
}
