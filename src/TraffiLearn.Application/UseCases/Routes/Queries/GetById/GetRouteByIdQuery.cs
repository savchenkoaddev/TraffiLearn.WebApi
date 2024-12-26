using MediatR;
using TraffiLearn.Application.UseCases.Routes.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Routes.Queries.GetById
{
    public sealed record GetRouteByIdQuery(
        Guid RouteId) : IRequest<Result<RouteResponse>>;
}
