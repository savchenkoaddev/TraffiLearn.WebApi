using MediatR;
using TraffiLearn.Application.ServiceCenters.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.ServiceCenters.Queries.GetAll
{
    public sealed record GetAllServiceCentersQuery
        : IRequest<Result<IEnumerable<ServiceCenterResponse>>>;
}
