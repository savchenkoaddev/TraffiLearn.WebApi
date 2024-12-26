using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.ServiceCenters.Commands.Delete
{
    public sealed record DeleteServiceCenterCommand(
        Guid ServiceCenterId) : IRequest<Result>;
}
