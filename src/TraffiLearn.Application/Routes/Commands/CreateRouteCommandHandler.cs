using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Routes;
using TraffiLearn.Domain.Aggregates.ServiceCenters;
using TraffiLearn.Domain.Aggregates.ServiceCenters.Errors;
using TraffiLearn.Domain.Aggregates.ServiceCenters.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Routes.Commands
{
    internal sealed class CreateRouteCommandHandler
        : IRequestHandler<CreateRouteCommand, Result<Guid>>
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IServiceCenterRepository _serviceCenterRepository;
        private readonly Mapper<CreateRouteCommand, Result<Route>> _requestMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRouteCommandHandler(
            IRouteRepository routeRepository,
            IServiceCenterRepository serviceCenterRepository,
            IUnitOfWork unitOfWork)
        {
            _routeRepository = routeRepository;
            _serviceCenterRepository = serviceCenterRepository;
            _requestMapper = null;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateRouteCommand request,
            CancellationToken cancellationToken)
        {
            var mappingResult = _requestMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return Result.Failure<Guid>(mappingResult.Error);
            }

            var serviceCenterId = new ServiceCenterId(request.ServiceCenterId.Value);

            var serviceCenter = await _serviceCenterRepository.GetByIdAsync(
                serviceCenterId,
                cancellationToken);

            if (serviceCenter is null)
            {
                return Result.Failure<Guid>(ServiceCenterErrors.NotFound);
            }

            var route = mappingResult.Value;

            /*
             * add route to the service center.
            */



            await _routeRepository.InsertAsync(
                route,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            throw new NotImplementedException();
        }
    }
}
