using FluentValidation;

namespace TraffiLearn.Application.ServiceCenters.Queries
{
    internal sealed class GetServiceCenterByIdQueryValidator
        : AbstractValidator<GetServiceCenterByIdQuery>
    {
        public GetServiceCenterByIdQueryValidator()
        {
            RuleFor(x => x.ServiceCenterId)
                .NotEmpty();
        }
    }
}
