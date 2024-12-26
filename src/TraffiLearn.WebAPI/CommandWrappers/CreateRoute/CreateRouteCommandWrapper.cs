using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;
using TraffiLearn.Application.UseCases.Routes.Commands.Create;

namespace TraffiLearn.WebAPI.CommandWrappers.CreateRoute
{
    public sealed class CreateRouteCommandWrapper
    {
        public required IFormFile Image { get; init; }

        [FromJson]
        public required CreateRouteRequest Request { get; init; }

        public CreateRouteCommand ToCommand()
        {
            return new CreateRouteCommand(
                ServiceCenterId: Request.ServiceCenterId,
                RouteNumber: Request.RouteNumber,
                Description: Request.Description,
                Image: Image);
        }
    }
}
