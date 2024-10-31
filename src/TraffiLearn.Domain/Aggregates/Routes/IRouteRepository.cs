using TraffiLearn.Domain.Aggregates.ServiceCenters;

namespace TraffiLearn.Domain.Aggregates.Routes
{
    public interface IRouteRepository
    {
        Task<IEnumerable<Route>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Route>> GetRoutesByServiceCenterId(
            ServiceCenterId serviceCenterId,
            CancellationToken cancellationToken = default);

        Task<Route?> GetByIdAsync(
            RouteId routeId,
            CancellationToken cancellationToken = default);

        Task<Route?> GetByIdWithServiceCenterAsync(
            RouteId routeId,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            Route route,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(Route route);

        Task DeleteAsync(Route route);
    }
}
