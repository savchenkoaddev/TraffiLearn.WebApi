using FluentValidation;
using TraffiLearn.Domain.Regions.RegionNames;

namespace TraffiLearn.Application.UseCases.Regions.Commands.Create
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
