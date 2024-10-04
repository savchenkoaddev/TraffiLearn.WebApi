using TraffiLearn.Domain.Aggregates.Regions;
using TraffiLearn.Domain.Aggregates.ServiceCenters.ValueObjects;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.ServiceCenters
{
    public sealed class ServiceCenter : AggregateRoot<ServiceCenterId>
    {
        private Address _address;
        private ServiceCenterNumber _number;
        private Region _region;

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

        public Result SetRegion(Region region)
        {
            ArgumentNullException.ThrowIfNull(region, nameof(region));

            Region = region;

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
