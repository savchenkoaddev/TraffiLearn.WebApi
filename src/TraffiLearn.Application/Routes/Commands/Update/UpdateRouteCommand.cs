using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Routes.Commands.Update
{
    public sealed record UpdateRouteCommand(
        Guid? RouteId,
        Guid? ServiceCenterId,
        int? RouteNumber,
        string? Description,
        IFormFile? Image) : IRequest<Result>;
}
