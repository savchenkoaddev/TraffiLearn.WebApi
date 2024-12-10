using FluentValidation;

namespace TraffiLearn.Application.UseCases.Regions.Commands.Delete
{
    internal sealed class DeleteRegionCommandValidator
        : AbstractValidator<DeleteRegionCommand>
    {
        public DeleteRegionCommandValidator()
        {
            RuleFor(x => x.RegionId)
                .NotEmpty();
        }
    }
}
