using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;
using TraffiLearn.Application.Routes.Commands.Update;

namespace TraffiLearn.WebAPI.CommandWrappers.UpdateRoute
{
    public sealed class UpdateRouteCommandWrapper
    {
        public IFormFile? Image { get; init; }

        [FromJson]
        public required UpdateRouteRequest Request { get; init; }

        public UpdateRouteCommand ToCommand()
        {
            return new UpdateRouteCommand(
                RouteId: Request.RouteId,
                ServiceCenterId: Request.ServiceCenterId,
                RouteNumber: Request.RouteNumber,
                Description: Request.Description,
                Image: Image);
        }
    }
}
