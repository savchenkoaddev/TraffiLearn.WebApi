using FluentValidation;

namespace TraffiLearn.Application.UseCases.Routes.Queries.GetById
{
    internal sealed class GetRouteByIdQueryValidator : AbstractValidator<GetRouteByIdQuery>
    {
        public GetRouteByIdQueryValidator()
        {
            RuleFor(x => x.RouteId)
                .NotEmpty();
        }
    }
}
