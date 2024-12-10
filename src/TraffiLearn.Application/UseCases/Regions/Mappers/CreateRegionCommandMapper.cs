using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Regions.Commands.Create;
using TraffiLearn.Domain.Regions;
using TraffiLearn.Domain.Regions.RegionNames;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Regions.Mappers
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
