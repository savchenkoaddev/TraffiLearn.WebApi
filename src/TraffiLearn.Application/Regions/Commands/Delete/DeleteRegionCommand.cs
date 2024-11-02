using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Regions.Commands.Delete
{
    public sealed record DeleteRegionCommand(
        Guid? RegionId) : IRequest<Result>;
}
