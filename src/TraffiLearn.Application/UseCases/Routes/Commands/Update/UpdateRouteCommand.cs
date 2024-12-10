using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Routes.Commands.Update
{
    public sealed record UpdateRouteCommand(
        Guid? RouteId,
        Guid? ServiceCenterId,
        int? RouteNumber,
        string? Description,
        IFormFile? Image) : IRequest<Result>;
}
