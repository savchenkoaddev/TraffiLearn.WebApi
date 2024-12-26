using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Routes.Commands.Update;
using TraffiLearn.Domain.Routes;
using TraffiLearn.Domain.Routes.RouteDescriptions;
using TraffiLearn.Domain.Routes.RouteNumbers;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Routes.Mappers
{
    internal sealed class UpdateRouteCommandToEntityMapper
        : Mapper<UpdateRouteCommand, Result<Route>>
    {
        public override Result<Route> Map(UpdateRouteCommand source)
        {
            var routeNumberResult = RouteNumber.Create(source.RouteNumber);

            if (routeNumberResult.IsFailure)
            {
                return Result.Failure<Route>(routeNumberResult.Error);
            }

            RouteDescription? description = default;

            if (source.Description is not null)
            {
                var descriptionResult = RouteDescription.Create(source.Description);

                if (descriptionResult.IsFailure)
                {
                    return Result.Failure<Route>(descriptionResult.Error);
                }

                description = descriptionResult.Value;
            }

            var routeId = new RouteId(source.RouteId);

            var routeResult = Route.Create(
                routeId,
                routeNumber: routeNumberResult.Value,
                routeDescription: description,
                imageUri: null);

            if (routeResult.IsFailure)
            {
                return Result.Failure<Route>(routeResult.Error);
            }

            return routeResult.Value;
        }
    }
}
