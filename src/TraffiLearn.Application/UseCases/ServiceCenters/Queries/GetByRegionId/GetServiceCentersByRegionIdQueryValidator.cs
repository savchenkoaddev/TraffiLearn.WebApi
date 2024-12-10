using FluentValidation;

namespace TraffiLearn.Application.UseCases.ServiceCenters.Queries.GetByRegionId
{
    internal sealed class GetServiceCentersByRegionIdQueryValidator
        : AbstractValidator<GetServiceCentersByRegionIdQuery>
    {
        public GetServiceCentersByRegionIdQueryValidator()
        {
            RuleFor(x => x.RegionId)
                .NotEmpty();
        }
    }
}
