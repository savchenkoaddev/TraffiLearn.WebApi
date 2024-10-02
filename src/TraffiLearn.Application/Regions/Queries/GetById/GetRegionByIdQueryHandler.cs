using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Regions.DTO;
using TraffiLearn.Domain.Aggregates.Regions;
using TraffiLearn.Domain.Aggregates.Regions.Errors;
using TraffiLearn.Domain.Aggregates.Regions.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Regions.Queries.GetById
{
    internal sealed class GetRegionByIdQueryHandler
        : IRequestHandler<GetRegionByIdQuery, Result<RegionResponse>>
    {
        private readonly IRegionRepository _regionRepository;
        private readonly Mapper<Region, RegionResponse> _mapper;

        public GetRegionByIdQueryHandler(
            IRegionRepository regionRepository,
            Mapper<Region, RegionResponse> mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        public async Task<Result<RegionResponse>> Handle(
            GetRegionByIdQuery request,
            CancellationToken cancellationToken)
        {
            var regionId = new RegionId(request.RegionId.Value);

            var region = await _regionRepository.GetByIdAsync(
                regionId,
                cancellationToken);

            if (region is null)
            {
                return Result.Failure<RegionResponse>(RegionErrors.NotFound);
            }

            return _mapper.Map(region);
        }
    }
}
