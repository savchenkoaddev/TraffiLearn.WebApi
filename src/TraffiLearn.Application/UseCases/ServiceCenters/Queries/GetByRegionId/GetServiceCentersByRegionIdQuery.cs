using MediatR;
using TraffiLearn.Application.UseCases.ServiceCenters.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.ServiceCenters.Queries.GetByRegionId
{
    public sealed record GetServiceCentersByRegionIdQuery(
        Guid RegionId) : IRequest<Result<IEnumerable<ServiceCenterResponse>>>;
}
