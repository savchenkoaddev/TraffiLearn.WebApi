using TraffiLearn.Infrastructure.Persistence;

namespace TraffiLearn.UnitTests.Abstractions
{
    public abstract class BaseRepositoryTest
    {
        protected readonly ApplicationDbContext DbContext;

        protected BaseRepositoryTest()
        {
            DbContext = ApplicationDbContextFactory.Create();
        }

        protected async Task AddRangeAndSaveAsync(params object[] entities)
        {
            await DbContext.AddRangeAsync(entities);
            await DbContext.SaveChangesAsync();
        }
    }
}
