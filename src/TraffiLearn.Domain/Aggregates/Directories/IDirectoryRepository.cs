namespace TraffiLearn.Domain.Aggregates.Directories
{
    public interface IDirectoryRepository
    {
        Task<IEnumerable<Directory>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<Directory?> GetByIdAsync(
            DirectoryId directoryId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            DirectoryId directoryId,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            Directory directory,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(Directory directory);

        Task DeleteAsync(Directory directory);
    }
}
