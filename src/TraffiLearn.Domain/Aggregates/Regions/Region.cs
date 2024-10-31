using TraffiLearn.Domain.Aggregates.Regions.RegionNames;
using TraffiLearn.Domain.Aggregates.ServiceCenters;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Regions
{
    public sealed class Region : AggregateRoot<RegionId>
    {
        private readonly HashSet<ServiceCenter> _serviceCenters = [];
        private RegionName _name;

        private Region()
            : base(new(Guid.Empty))
        { }

        private Region(
            RegionId regionId,
            RegionName name) : base(regionId)
        {
            Name = name;
        }

        public RegionName Name
        {
            get
            {
                return _name;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value, "RegionName cannot be null");

                _name = value;
            }
        }

        public IReadOnlyCollection<ServiceCenter> ServiceCenters => _serviceCenters;

        public Result Update(
            RegionName name)
        {
            Name = name;

            return Result.Success();
        }

        public static Result<Region> Create(
            RegionId regionId,
            RegionName name)
        {
            return new Region(regionId, name);
        }
    }
}
