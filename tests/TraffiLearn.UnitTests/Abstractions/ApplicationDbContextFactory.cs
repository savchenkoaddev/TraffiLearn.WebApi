using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
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

            var mockPublisher = new Mock<IPublisher>();

            return new ApplicationDbContext(options, mockPublisher.Object);
        }
    }
}
