using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Aggregates.Regions.ValueObjects;
using TraffiLearn.Domain.Aggregates.ServiceCenters;
using TraffiLearn.Domain.Aggregates.ServiceCenters.ValueObjects;

namespace TraffiLearn.Infrastructure.Persistence.Repositories
{
    internal sealed class ServiceCenterRepository : IServiceCenterRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ServiceCenterRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ServiceCenter>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.ServiceCenters
                .ToListAsync(cancellationToken);
        }

        public async Task<ServiceCenter?> GetByIdAsync(
            ServiceCenterId serviceCenterId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.ServiceCenters
                .FindAsync(
                    keyValues: [serviceCenterId],
                    cancellationToken);
        }

        public async Task<IEnumerable<ServiceCenter>> GetServiceCentersByRegionId(
            RegionId regionId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.ServiceCenters
                .Where(sc => sc.Region.Id == regionId)
                .ToListAsync(cancellationToken);
        }

        public async Task InsertAsync(
            ServiceCenter serviceCenter,
            CancellationToken cancellationToken = default)
        {
            EnsureServiceCenterIsNotNull(serviceCenter);

            await _dbContext.ServiceCenters.AddAsync(
                serviceCenter,
                cancellationToken);
        }

        public Task UpdateAsync(ServiceCenter serviceCenter)
        {
            EnsureServiceCenterIsNotNull(serviceCenter);

            _dbContext.ServiceCenters.Update(serviceCenter);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(ServiceCenter serviceCenter)
        {
            EnsureServiceCenterIsNotNull(serviceCenter);

            _dbContext.ServiceCenters.Remove(serviceCenter);

            return Task.CompletedTask;
        }

        private static void EnsureServiceCenterIsNotNull(ServiceCenter serviceCenter)
        {
            ArgumentNullException.ThrowIfNull(serviceCenter, nameof(serviceCenter));
        }

        public async Task<bool> ExistsAsync(
            ServiceCenterId serviceCenterId, 
            CancellationToken cancellationToken = default)
        {
            return await GetByIdAsync(serviceCenterId, cancellationToken) is not null;
        }
    }
}
