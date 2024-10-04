using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.ServiceCenters.Commands.Create
{
    public sealed record CreateServiceCenterCommand(
        Guid? RegionId,
        string? LocationName,
        string? RoadName,
        string? BuildingNumber) : IRequest<Result<Guid>>;
}
