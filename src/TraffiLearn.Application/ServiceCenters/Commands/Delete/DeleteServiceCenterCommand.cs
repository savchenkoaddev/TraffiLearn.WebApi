using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.ServiceCenters.Commands.Delete
{
    public sealed record DeleteServiceCenterCommand(
        Guid? ServiceCenterId) : IRequest<Result>;
}
