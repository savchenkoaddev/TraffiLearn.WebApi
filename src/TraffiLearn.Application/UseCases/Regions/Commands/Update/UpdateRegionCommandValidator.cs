﻿using FluentValidation;
using TraffiLearn.Domain.Regions.RegionNames;

namespace TraffiLearn.Application.UseCases.Regions.Commands.Update
{
    internal sealed class UpdateRegionCommandValidator
        : AbstractValidator<UpdateRegionCommand>
    {
        public UpdateRegionCommandValidator()
        {
            RuleFor(x => x.RegionId)
                .NotEmpty();

            RuleFor(x => x.RegionName)
                .NotEmpty()
                .MaximumLength(RegionName.MaxLength);
        }
    }
}
