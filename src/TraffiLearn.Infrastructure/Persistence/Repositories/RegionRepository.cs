using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Aggregates.Regions;
using TraffiLearn.Domain.Aggregates.Regions.ValueObjects;

namespace TraffiLearn.Infrastructure.Persistence.Repositories
{
    internal sealed class RegionRepository : IRegionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RegionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task DeleteAsync(Region region)
        {
            EnsurePassedRegionIsNotNull(region);

            _dbContext.Regions.Remove(region);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(
            RegionId regionId,
            CancellationToken cancellationToken = default)
        {
            return await GetByIdAsync(
                regionId,
                cancellationToken) is not null;
        }

        public async Task<IEnumerable<Region>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Regions.ToListAsync(cancellationToken);
        }

        public async Task<Region?> GetByIdAsync(
            RegionId regionId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Regions.FindAsync(
                keyValues: [regionId],
                cancellationToken);
        }

        public async Task InsertAsync(
            Region region,
            CancellationToken cancellationToken = default)
        {
            EnsurePassedRegionIsNotNull(region);

            await _dbContext.Regions.AddAsync(
                region,
                cancellationToken);
        }

        public Task UpdateAsync(Region region)
        {
            EnsurePassedRegionIsNotNull(region);

            _dbContext.Regions.Update(region);

            return Task.CompletedTask;
        }

        private static void EnsurePassedRegionIsNotNull(Region region)
        {
            ArgumentNullException.ThrowIfNull(region, "Region cannot be null");
        }
    }
}
