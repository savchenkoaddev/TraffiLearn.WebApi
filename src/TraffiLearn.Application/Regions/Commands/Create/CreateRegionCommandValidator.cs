using FluentValidation;
using TraffiLearn.Domain.Aggregates.Regions.ValueObjects;

namespace TraffiLearn.Application.Regions.Commands.Create
{
    internal sealed class CreateRegionCommandValidator
        : AbstractValidator<CreateRegionCommand>
    {
        public CreateRegionCommandValidator()
        {
            RuleFor(x => x.RegionName)
                .NotEmpty()
                .MaximumLength(RegionName.MaxLength);
        }
    }
}
