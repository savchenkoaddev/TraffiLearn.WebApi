using FluentValidation;

namespace TraffiLearn.Application.Routes.Queries.GetByServiceCenterId
{
    internal sealed class GetRoutesByServiceCenterIdQueryValidator
        : AbstractValidator<GetRoutesByServiceCenterIdQuery>
    {
        public GetRoutesByServiceCenterIdQueryValidator()
        {
            RuleFor(x => x.ServiceCenterId)
                .NotEmpty();
        }
    }
}
