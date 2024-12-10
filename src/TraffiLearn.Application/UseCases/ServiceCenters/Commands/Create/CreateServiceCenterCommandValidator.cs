using FluentValidation;
using TraffiLearn.Domain.ServiceCenters.BuildingNumbers;
using TraffiLearn.Domain.ServiceCenters.LocationNames;
using TraffiLearn.Domain.ServiceCenters.RoadNames;
using TraffiLearn.Domain.ServiceCenters.ServiceCenterNumbers;

namespace TraffiLearn.Application.UseCases.ServiceCenters.Commands.Create
{
    internal sealed class CreateServiceCenterCommandValidator
        : AbstractValidator<CreateServiceCenterCommand>
    {
        public CreateServiceCenterCommandValidator()
        {
            RuleFor(x => x.RegionId)
                .NotEmpty();

            RuleFor(x => x.ServiceCenterNumber)
                .NotEmpty()
                .MaximumLength(ServiceCenterNumber.MaxLength);

            RuleFor(x => x.ServiceCenterNumber)
                .Must(number => int.TryParse(number, out _) == true)
                .WithMessage("ServiceCenterNumber must be a number.");

            RuleFor(x => x.LocationName)
                .NotEmpty()
                .MaximumLength(LocationName.MaxLength);

            RuleFor(x => x.RoadName)
                .NotEmpty()
                .MaximumLength(RoadName.MaxLength);

            RuleFor(x => x.BuildingNumber)
                .NotEmpty()
                .MaximumLength(BuildingNumber.MaxLength);
        }
    }
}
