using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Regions.DTO;
using TraffiLearn.Domain.Regions;

namespace TraffiLearn.Application.UseCases.Regions.Mappers
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
