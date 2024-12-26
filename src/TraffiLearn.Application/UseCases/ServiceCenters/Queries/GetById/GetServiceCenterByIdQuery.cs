using MediatR;
using TraffiLearn.Application.UseCases.ServiceCenters.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.ServiceCenters.Queries.GetById
{
    public sealed record GetServiceCenterByIdQuery(
        Guid ServiceCenterId) : IRequest<Result<ServiceCenterResponse>>;
}
