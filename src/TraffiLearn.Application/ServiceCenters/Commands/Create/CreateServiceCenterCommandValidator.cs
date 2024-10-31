using FluentValidation;
using TraffiLearn.Domain.Aggregates.ServiceCenters.BuildingNumbers;
using TraffiLearn.Domain.Aggregates.ServiceCenters.LocationNames;
using TraffiLearn.Domain.Aggregates.ServiceCenters.RoadNames;
using TraffiLearn.Domain.Aggregates.ServiceCenters.ServiceCenterNumbers;

namespace TraffiLearn.Application.ServiceCenters.Commands.Create
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
