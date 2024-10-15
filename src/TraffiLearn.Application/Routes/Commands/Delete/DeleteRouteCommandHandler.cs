using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Routes;
using TraffiLearn.Domain.Aggregates.Routes.Errors;
using TraffiLearn.Domain.Aggregates.Routes.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Routes.Commands.Delete
{
    internal sealed class DeleteRouteCommandHandler
        : IRequestHandler<DeleteRouteCommand, Result>
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRouteCommandHandler(
            IRouteRepository routeRepository, 
            IUnitOfWork unitOfWork)
        {
            _routeRepository = routeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DeleteRouteCommand request, 
            CancellationToken cancellationToken)
        {
            var routeId = new RouteId(request.RouteId.Value);

            var route = await _routeRepository.GetByIdAsync(routeId, cancellationToken);

            if (route is null)
            {
                return RouteErrors.NotFound;
            }

            await _routeRepository.DeleteAsync(route);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
    }
}
