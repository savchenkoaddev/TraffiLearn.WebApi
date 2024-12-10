using FluentValidation;

namespace TraffiLearn.Application.UseCases.Routes.Queries.GetByServiceCenterId
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
