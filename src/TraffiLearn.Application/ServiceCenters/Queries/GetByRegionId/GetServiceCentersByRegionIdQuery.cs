using MediatR;
using TraffiLearn.Application.ServiceCenters.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.ServiceCenters.Queries.GetByRegionId
{
    public sealed record GetServiceCentersByRegionIdQuery(
        Guid? RegionId) : IRequest<Result<IEnumerable<ServiceCenterResponse>>>;
}
