using MediatR;
using TraffiLearn.Application.ServiceCenters.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.ServiceCenters.Queries.GetById
{
    public sealed record GetServiceCenterByIdQuery(
        Guid? ServiceCenterId) : IRequest<Result<ServiceCenterResponse>>;
}
