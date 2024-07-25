using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Infrastructure.Database;

namespace TraffiLearn.Infrastructure.Repositories
{
    internal sealed class TopicRepository : Repository<Topic>, ITopicRepository
    {
        public TopicRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        { }
    }
}
