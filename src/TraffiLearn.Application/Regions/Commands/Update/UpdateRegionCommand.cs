using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Regions.Commands.Update
{
    public sealed record UpdateRegionCommand(
        Guid? RegionId,
        string? RegionName) : IRequest<Result>;
}
