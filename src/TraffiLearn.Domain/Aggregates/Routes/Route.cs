using TraffiLearn.Domain.Aggregates.Common.ImageUri;
using TraffiLearn.Domain.Aggregates.Routes.ValueObjects;
using TraffiLearn.Domain.Aggregates.ServiceCenters;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Routes
{
    public sealed class Route : AggregateRoot<RouteId>
    {
        private RouteNumber _routeNumber;
        private RouteDescription _description;
        private ImageUri _imageUri;
        private ServiceCenter _serviceCenter;

        public Route()
            : base(new(Guid.Empty))
        { }

        private Route(
            RouteId id,
            RouteNumber routeNumber,
            RouteDescription description,
            ImageUri imageUri) : base(id)
        {
            RouteNumber = routeNumber;
            Description = description;
            ImageUri = imageUri;
        }

        public RouteNumber RouteNumber
        {
            get
            {
                return _routeNumber;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value, "Route number cannot be null.");

                _routeNumber = value;
            }
        }

        public RouteDescription Description
        {
            get
            {
                return _description;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value, "Route description cannot be null.");

                _description = value;
            }
        }

        public ImageUri ImageUri
        {
            get
            {
                return _imageUri;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value, "Image URI cannot be null.");

                _imageUri = value;
            }
        }

        public ServiceCenter? ServiceCenter { get; private set; } = default;

        public Result SetServiceCenter(ServiceCenter serviceCenter)
        {
            ArgumentNullException.ThrowIfNull(serviceCenter, nameof(serviceCenter));

            ServiceCenter = serviceCenter;

            return Result.Success();
        }

        public Result Update(
            RouteNumber routeNumber,
            RouteDescription routeDescription,
            ImageUri imageUri)
        {
            RouteNumber = routeNumber;
            Description = routeDescription;
            ImageUri = imageUri;

            return Result.Success();
        }

        public static Result<Route> Create(
            RouteId routeId,
            RouteNumber routeNumber,
            RouteDescription routeDescription,
            ImageUri imageUri)
        {
            return new Route(routeId, routeNumber, routeDescription, imageUri);
        }
    }
}
