using TraffiLearn.Domain.Aggregates.Regions;

namespace TraffiLearn.Domain.Aggregates.ServiceCenters
{
    public interface IServiceCenterRepository
    {
        Task<IEnumerable<ServiceCenter>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<IEnumerable<ServiceCenter>> GetServiceCentersByRegionId(
            RegionId regionId,
            CancellationToken cancellationToken = default);

        Task<ServiceCenter?> GetByIdAsync(
            ServiceCenterId serviceCenterId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            ServiceCenterId serviceCenterId,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            ServiceCenter serviceCenter,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(ServiceCenter serviceCenter);

        Task DeleteAsync(ServiceCenter serviceCenter);
    }
}
