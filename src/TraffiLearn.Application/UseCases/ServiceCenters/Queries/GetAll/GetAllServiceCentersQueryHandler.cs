using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.ServiceCenters.DTO;
using TraffiLearn.Domain.ServiceCenters;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.ServiceCenters.Queries.GetAll
{
    internal sealed class GetAllServiceCentersQueryHandler
        : IRequestHandler<GetAllServiceCentersQuery,
            Result<IEnumerable<ServiceCenterResponse>>>
    {
        private readonly IServiceCenterRepository _serviceCenterRepository;
        private readonly Mapper<ServiceCenter, ServiceCenterResponse> _responseMapper;

        public GetAllServiceCentersQueryHandler(
            IServiceCenterRepository serviceCenterRepository,
            Mapper<ServiceCenter, ServiceCenterResponse> responseMapper)
        {
            _serviceCenterRepository = serviceCenterRepository;
            _responseMapper = responseMapper;
        }

        public async Task<Result<IEnumerable<ServiceCenterResponse>>> Handle(
            GetAllServiceCentersQuery request,
            CancellationToken cancellationToken)
        {
            var serviceCenters = await _serviceCenterRepository.GetAllAsync(
                cancellationToken);

            return Result.Success(_responseMapper.Map(serviceCenters));
        }
    }
}
