using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Regions.Commands.Delete
{
    public sealed record DeleteRegionCommand(
        Guid RegionId) : IRequest<Result>;
}
