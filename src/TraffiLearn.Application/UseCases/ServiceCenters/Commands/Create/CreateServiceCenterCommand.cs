using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.ServiceCenters.Commands.Create
{
    public sealed record CreateServiceCenterCommand(
        Guid RegionId,
        string ServiceCenterNumber,
        string LocationName,
        string RoadName,
        string BuildingNumber) : IRequest<Result<Guid>>;
}
