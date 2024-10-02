using TraffiLearn.Domain.Aggregates.Regions.ValueObjects;

namespace TraffiLearn.Domain.Aggregates.Regions
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<Region?> GetByIdAsync(
            RegionId regionId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            RegionId regionId,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            Region region,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(Region region);

        Task DeleteAsync(Region region);
    }
}
