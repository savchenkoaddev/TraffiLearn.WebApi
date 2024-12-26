using FluentValidation;
using TraffiLearn.Application.Validators;
using TraffiLearn.Domain.Routes.RouteDescriptions;
using TraffiLearn.Domain.Routes.RouteNumbers;

namespace TraffiLearn.Application.UseCases.Routes.Commands.Update
{
    internal sealed class UpdateRouteCommandValidator
        : AbstractValidator<UpdateRouteCommand>
    {
        public UpdateRouteCommandValidator()
        {
            RuleFor(x => x.RouteId)
                .NotEmpty();

            RuleFor(x => x.ServiceCenterId)
                .NotEmpty();

            RuleFor(x => x.RouteNumber)
                .NotEmpty()
                .GreaterThanOrEqualTo(RouteNumber.MinValue);

            RuleFor(x => x.Description)
                .MaximumLength(RouteDescription.MaxLength)
                .When(x => x.Description is not null);

            RuleFor(x => x.Image)
                .SetValidator(new ImageValidator())
                .When(x => x.Image is not null);
        }
    }
}
