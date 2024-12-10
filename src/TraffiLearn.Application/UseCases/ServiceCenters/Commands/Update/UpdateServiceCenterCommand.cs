using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.ServiceCenters.Commands.Update
{
    public sealed record UpdateServiceCenterCommand(
        Guid? ServiceCenterId,
        Guid? RegionId,
        string? ServiceCenterNumber,
        string? LocationName,
        string? RoadName,
        string? BuildingNumber) : IRequest<Result>;
}
