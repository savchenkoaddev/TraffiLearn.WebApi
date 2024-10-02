using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Regions.DTO;
using TraffiLearn.Domain.Aggregates.Regions;

namespace TraffiLearn.Application.Regions.Mappers
{
    internal sealed class RegionToRegionResponseMapper
        : Mapper<Region, RegionResponse>
    {
        public override RegionResponse Map(Region source)
        {
            return new RegionResponse(
                Id: source.Id.Value,
                Name: source.Name.Value);
        }
    }
}
