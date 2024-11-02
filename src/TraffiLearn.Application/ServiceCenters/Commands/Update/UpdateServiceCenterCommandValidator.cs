using FluentValidation;
using TraffiLearn.Domain.ServiceCenters.BuildingNumbers;
using TraffiLearn.Domain.ServiceCenters.LocationNames;
using TraffiLearn.Domain.ServiceCenters.RoadNames;
using TraffiLearn.Domain.ServiceCenters.ServiceCenterNumbers;

namespace TraffiLearn.Application.ServiceCenters.Commands.Update
{
    internal sealed class UpdateServiceCenterCommandValidator
        : AbstractValidator<UpdateServiceCenterCommand>
    {
        public UpdateServiceCenterCommandValidator()
        {
            RuleFor(x => x.ServiceCenterId)
                .NotEmpty();

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
