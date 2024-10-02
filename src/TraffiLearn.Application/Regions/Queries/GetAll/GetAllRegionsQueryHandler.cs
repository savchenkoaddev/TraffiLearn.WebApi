using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Regions.DTO;
using TraffiLearn.Domain.Aggregates.Regions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Regions.Queries.GetAll
{
    internal sealed class GetAllRegionsQueryHandler
        : IRequestHandler<GetAllRegionsQuery, Result<IEnumerable<RegionResponse>>>
    {
        private readonly IRegionRepository _regionRepository;
        private readonly Mapper<Region, RegionResponse> _regionMapper;

        public GetAllRegionsQueryHandler(
            IRegionRepository regionRepository, 
            Mapper<Region, RegionResponse> regionMapper)
        {
            _regionRepository = regionRepository;
            _regionMapper = regionMapper;
        }

        public async Task<Result<IEnumerable<RegionResponse>>> Handle(
            GetAllRegionsQuery request, 
            CancellationToken cancellationToken)
        {
            var regions = await _regionRepository.GetAllRegionsAsync(
                cancellationToken);

            return Result.Success(_regionMapper.Map(regions));
        }
    }
}
