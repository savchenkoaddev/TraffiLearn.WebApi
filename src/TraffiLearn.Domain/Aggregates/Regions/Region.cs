using TraffiLearn.Domain.Aggregates.Regions.ValueObjects;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Regions
{
    public sealed class Region : AggregateRoot<RegionId>
    {
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
            set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));

                _name = value;
            }
        }

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
