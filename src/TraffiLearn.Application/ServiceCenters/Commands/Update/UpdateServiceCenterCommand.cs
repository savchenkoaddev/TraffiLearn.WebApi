﻿using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.ServiceCenters.Commands.Update
{
    public sealed record UpdateServiceCenterCommand(
        Guid? ServiceCenterId,
        Guid? RegionId,
        string? ServiceCenterNumber,
        string? LocationName,
        string? RoadName,
        string? BuildingNumber) : IRequest<Result>;
}
