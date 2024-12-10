using FluentValidation;

namespace TraffiLearn.Application.UseCases.Regions.Queries.GetById
{
    internal sealed class GetRegionByIdQueryValidator
        : AbstractValidator<GetRegionByIdQuery>
    {
        public GetRegionByIdQueryValidator()
        {
            RuleFor(x => x.RegionId)
                .NotEmpty();
        }
    }
}
