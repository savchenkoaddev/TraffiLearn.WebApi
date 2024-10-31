using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Regions.Commands.Create;
using TraffiLearn.Domain.Aggregates.Regions;
using TraffiLearn.Domain.Aggregates.Regions.RegionNames;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Regions.Mappers
{
    internal sealed class CreateRegionCommandMapper
        : Mapper<CreateRegionCommand, Result<Region>>
    {
        public override Result<Region> Map(CreateRegionCommand source)
        {
            var regionNameResult = RegionName.Create(source.RegionName);

            if (regionNameResult.IsFailure)
            {
                return Result.Failure<Region>(regionNameResult.Error);
            }

            var regionId = new RegionId(Guid.NewGuid());

            var result = Region.Create(
                regionId,
                name: regionNameResult.Value);

            if (result.IsFailure)
            {
                return Result.Failure<Region>(result.Error);
            }

            return result.Value;
        }
    }
}
