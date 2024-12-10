using MediatR;
using TraffiLearn.Application.UseCases.Routes.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Routes.Queries.GetByServiceCenterId
{
    public sealed record GetRoutesByServiceCenterIdQuery(
        Guid? ServiceCenterId) : IRequest<Result<IEnumerable<RouteResponse>>>;
}