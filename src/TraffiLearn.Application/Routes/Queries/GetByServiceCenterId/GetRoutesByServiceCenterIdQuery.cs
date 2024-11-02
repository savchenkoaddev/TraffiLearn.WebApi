using MediatR;
using TraffiLearn.Application.Routes.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Routes.Queries.GetByServiceCenterId
{
    public sealed record GetRoutesByServiceCenterIdQuery(
        Guid? ServiceCenterId) : IRequest<Result<IEnumerable<RouteResponse>>>;
}