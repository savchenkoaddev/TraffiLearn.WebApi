using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Aggregates.Routes;
using TraffiLearn.Domain.Aggregates.Routes.ValueObjects;
using TraffiLearn.Domain.Aggregates.ServiceCenters.ValueObjects;

namespace TraffiLearn.Infrastructure.Persistence.Repositories
{
    public sealed class RouteRepository : IRouteRepository
    {
        private ApplicationDbContext _dbContext;

        public RouteRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Route>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Routes
                .ToListAsync(cancellationToken);
        }

        public async Task<Route?> GetByIdAsync(
            RouteId routeId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Routes
                .FindAsync(
                    keyValues: [routeId],
                    cancellationToken);
        }

        public async Task<IEnumerable<Route>> GetRoutesByServiceCenterId(
            ServiceCenterId serviceCenterId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Routes
                .Where(route => route.ServiceCenter.Id == serviceCenterId)
                .ToListAsync(cancellationToken);
        }

        public async Task InsertAsync(
            Route route,
            CancellationToken cancellationToken = default)
        {
            EnsureRouteIsNotNull(route);

            await _dbContext.Routes.AddAsync(
                route,
                cancellationToken);
        }

        public Task UpdateAsync(Route route)
        {
            EnsureRouteIsNotNull(route);

            _dbContext.Routes.Update(route);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Route route)
        {
            EnsureRouteIsNotNull(route);

            _dbContext.Routes.Remove(route);

            return Task.CompletedTask;
        }

        private static void EnsureRouteIsNotNull(Route route)
        {
            ArgumentNullException.ThrowIfNull(route, nameof(route));
        }
    }
}
