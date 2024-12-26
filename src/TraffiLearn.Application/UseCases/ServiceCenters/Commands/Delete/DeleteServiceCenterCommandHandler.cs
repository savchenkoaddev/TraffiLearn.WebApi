using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.ServiceCenters;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.ServiceCenters.Commands.Delete
{
    internal sealed class DeleteServiceCenterCommandHandler
        : IRequestHandler<DeleteServiceCenterCommand, Result>
    {
        private readonly IServiceCenterRepository _serviceCenterRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteServiceCenterCommandHandler(
            IServiceCenterRepository serviceCenterRepository,
            IUnitOfWork unitOfWork)
        {
            _serviceCenterRepository = serviceCenterRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DeleteServiceCenterCommand request,
            CancellationToken cancellationToken)
        {
            var serviceCenterId = new ServiceCenterId(request.ServiceCenterId);

            var serviceCenter = await _serviceCenterRepository.GetByIdAsync(
                serviceCenterId,
                cancellationToken);

            if (serviceCenter is null)
            {
                return ServiceCenterErrors.NotFound;
            }

            await _serviceCenterRepository.DeleteAsync(serviceCenter);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
