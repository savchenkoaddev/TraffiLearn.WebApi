using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Regions.Commands.Update
{
    public sealed record UpdateRegionCommand(
        Guid? RegionId,
        string? RegionName) : IRequest<Result>;
}
