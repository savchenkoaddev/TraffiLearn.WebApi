using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Directories;
using Directory = TraffiLearn.Domain.Directories.Directory;

namespace TraffiLearn.Infrastructure.Persistence.Repositories
{
    internal sealed class DirectoryRepository : IDirectoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DirectoryRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public Task DeleteAsync(Directory directory)
        {
            EnsurePassedDirectoryIsNotNull(directory);

            _dbContext.Directories.Remove(directory);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(
            DirectoryId directoryId,
            CancellationToken cancellationToken = default)
        {
            return await GetByIdAsync(
                directoryId,
                cancellationToken) is not null;
        }

        public async Task<IEnumerable<Directory>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Directories.ToListAsync(cancellationToken);
        }

        public async Task<Directory?> GetByIdAsync(
            DirectoryId directoryId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Directories.FindAsync(
                keyValues: [directoryId],
                cancellationToken);
        }

        public async Task InsertAsync(
            Directory directory,
            CancellationToken cancellationToken = default)
        {
            EnsurePassedDirectoryIsNotNull(directory);

            await _dbContext.Directories.AddAsync(
                directory,
                cancellationToken);
        }

        public Task UpdateAsync(Directory directory)
        {
            EnsurePassedDirectoryIsNotNull(directory);

            _dbContext.Directories.Update(directory);

            return Task.CompletedTask;
        }

        private void EnsurePassedDirectoryIsNotNull(Directory directory)
        {
            ArgumentNullException.ThrowIfNull(directory, "Directory can't be null");
        }
    }
}
