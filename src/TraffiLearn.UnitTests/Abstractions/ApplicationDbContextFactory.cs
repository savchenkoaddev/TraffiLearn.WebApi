using Microsoft.EntityFrameworkCore;
using TraffiLearn.Infrastructure.Persistence;

namespace TraffiLearn.UnitTests.Abstractions
{
    public static class ApplicationDbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
