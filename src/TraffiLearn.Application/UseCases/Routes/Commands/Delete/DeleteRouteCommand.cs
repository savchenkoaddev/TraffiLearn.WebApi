using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Routes.Commands.Delete
{
    public sealed record DeleteRouteCommand(
        Guid? RouteId) : IRequest<Result>;
}
