using TraffiLearn.Domain.Regions;
using TraffiLearn.Domain.Routes;
using TraffiLearn.Domain.ServiceCenters.Addresses;
using TraffiLearn.Domain.ServiceCenters.ServiceCenterNumbers;
using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.ServiceCenters
{
    public sealed class ServiceCenter : AggregateRoot<ServiceCenterId>
    {
        private readonly HashSet<Route> _routes = [];
        private Address _address;
        private ServiceCenterNumber _number;

        private ServiceCenter()
            : base(new(Guid.Empty))
        { }

        private ServiceCenter(
            ServiceCenterId id,
            Address address,
            ServiceCenterNumber number)
            : base(id)
        {
            Address = address;
            Number = number;
        }

        public Address Address
        {
            get
            {
                return _address;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value, "Address cannot be null.");

                _address = value;
            }
        }

        public ServiceCenterNumber Number
        {
            get
            {
                return _number;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value, "Service center number cannot be null.");

                _number = value;
            }
        }

        public Region? Region { get; private set; } = default;

        public IReadOnlyCollection<Route> Routes => _routes;

        public Result SetRegion(Region region)
        {
            ArgumentNullException.ThrowIfNull(region, nameof(region));

            Region = region;

            return Result.Success();
        }

        public Result AddRoute(Route route)
        {
            ArgumentNullException.ThrowIfNull(route, nameof(route));

            if (_routes.Contains(route))
            {
                return ServiceCenterErrors.RouteAlreadyAdded;
            }

            _routes.Add(route);

            return Result.Success();
        }

        public Result RemoveRoute(Route route)
        {
            ArgumentNullException.ThrowIfNull(route, nameof(route));

            if (!_routes.Contains(route))
            {
                return ServiceCenterErrors.RouteNotFound;
            }

            _routes.Remove(route);

            return Result.Success();
        }

        public Result Update(
            Address address,
            ServiceCenterNumber number)
        {
            Address = address;
            Number = number;

            return Result.Success();
        }

        public static Result<ServiceCenter> Create(
            ServiceCenterId id,
            Address address,
            ServiceCenterNumber number)
        {
            return new ServiceCenter(id, address, number);
        }
    }
}
