using MediatR;
using TraffiLearn.Application.UseCases.ServiceCenters.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.ServiceCenters.Queries.GetAll
{
    public sealed record GetAllServiceCentersQuery
        : IRequest<Result<IEnumerable<ServiceCenterResponse>>>;
}
